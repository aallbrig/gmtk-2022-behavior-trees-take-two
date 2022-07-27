using System;
using Model;
using Model.AI.BehaviorTrees;
using Model.AI.BehaviorTrees.BuildingBlocks;
using Model.Interfaces;
using ScriptableObjects.Agent;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace MonoBehaviours
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Grunt : MonoBehaviour, ITrackTargets, IBehaviorTreeProvider
    {
        private static int _maxEnemyColliders = 3;
        public event Action<Target> TargetAcquired;
        public event Action TargetLost;
        private NavMeshAgent _agent;

        private IAgentConfiguration _agentConfig;
        private float _timeOfLastThought;
        private Transform _target;
        private BehaviorTree _brain;
        private readonly Collider[] _enemyColliders = new Collider[_maxEnemyColliders];
        #if UNITY_EDITOR
        private Color _agentGizmoColor;
        #endif

        public BehaviorTree ProvideBehaviorTree()
        {
            var hasNoTarget = new Conditional(new ConditionalContext(HasTarget));
            var moveToTarget = new TaskAction(new TaskActionContext(MoveToTarget));
            // c: have target -> move to target

            var seq = new Sequence();
            seq.AddChild(hasNoTarget);
            seq.AddChild(moveToTarget);

            var bt = new BehaviorTree(seq);
            BehaviorTreeProvided?.Invoke(bt);
            return bt;
        }

        private void Start()
        {
            _agentConfig ??= ScriptableObject.CreateInstance<AgentConfiguration>();

            _agent ??= GetComponent<NavMeshAgent>();
            _agent.speed = _agentConfig.WalkSpeed;

            _brain = ProvideBehaviorTree();
            // let the brain think instantly
            _timeOfLastThought = Time.time - _agentConfig.ThinkRate;
            #if UNITY_EDITOR
            _agentGizmoColor = new Color(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f)
            );
            #endif
        }

        private void Update()
        {
            DetectEnemy(); // TODO: I think a sensory item should be contained in the BT
            Think();
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            _agentConfig ??= ScriptableObject.CreateInstance<AgentConfiguration>();

            var transformPosition = transform.position;
            if (_target)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transformPosition, _agentConfig.AttackRange);
            }
            else
            {
                Gizmos.color = _agentGizmoColor;
                Gizmos.DrawWireSphere(transformPosition, _agentConfig.DetectRange);
            }
        }
        #endif

        private void Think()
        {
            if (Time.time - _timeOfLastThought > _agentConfig.ThinkRate)
            {
                _brain.Run();
                _timeOfLastThought = Time.time;
            }
        }

        public void SetTarget(Target target)
        {
            _target = target.Transform ? target.Transform : throw new ArgumentNullException(nameof(target));
            TargetAcquired?.Invoke(target);
        }

        public event Action<BehaviorTree> BehaviorTreeProvided;

        private void DetectEnemy()
        {
            if (_target != null)
            {
                // make sure target is outside detect range
                if (Vector3.Distance(_target.position, transform.position) > _agentConfig.DetectRange)
                {
                    _target = null;
                    TargetLost?.Invoke();
                }
            }

            var numCollisions = Physics.OverlapSphereNonAlloc(
                transform.position,
                _agentConfig.DetectRange,
                _enemyColliders,
                1<<_agentConfig.EnemyLayer
            );

            if (numCollisions > 0)
                SetTarget(new Target(_enemyColliders[0].transform)); // TODO: re-evaluate this "pick first" strategy
        }

        private bool HasTarget() => _target != null;

        private Status MoveToTarget()
        {
            if (HasTarget() == false)
                return Status.Failure;
            if (Vector3.Distance(_target.position, transform.position) <= _agentConfig.AttackRange)
                return Status.Success;

            if (_agent.isOnNavMesh)
            {
                _agent.SetDestination(_target.position);
                return Status.Running;
            } else return Status.Failure;
        }
    }
}
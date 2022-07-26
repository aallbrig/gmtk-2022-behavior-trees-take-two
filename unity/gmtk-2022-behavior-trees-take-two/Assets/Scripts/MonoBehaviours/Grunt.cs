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
    public class Grunt : MonoBehaviour, ITrackTargets, IBehaviorTreeProvider
    {
        private static int _maxEnemyColliders = 3;
        public event Action<Target> TargetAcquired;
        public event Action TargetLost;
        public NavMeshAgent agent;

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
            var hasNoTarget = new Conditional(new ConditionalContext(() => !HasTarget()));
            // c: enemy within range -> set target
            var detectEnemy = new Conditional(new ConditionalContext(EnemyWithinRange));
            var moveToTarget = new TaskAction(new TaskActionContext(MoveToTarget));
            // c: have target -> move to target

            var seq = new Sequence();
            seq.AddChild(hasNoTarget);
            seq.AddChild(detectEnemy);
            seq.AddChild(moveToTarget);

            var bt = new BehaviorTree(seq);
            BehaviorTreeProvided?.Invoke(bt);
            return bt;
        }

        private void Start()
        {
            _agentConfig ??= ScriptableObject.CreateInstance<AgentConfiguration>();

            agent ??= GetComponent<NavMeshAgent>();
            if (agent == null) throw new NullReferenceException();
            agent.speed = _agentConfig.WalkSpeed;

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
            Think();
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            _agentConfig ??= ScriptableObject.CreateInstance<AgentConfiguration>();

            var transformPosition = transform.position;
            Gizmos.color = _agentGizmoColor;
            Gizmos.DrawWireSphere(transformPosition, _agentConfig.DetectRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transformPosition, _agentConfig.AttackRange);
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
            var numCollisions = Physics.OverlapSphereNonAlloc(
                transform.position,
                _agentConfig.DetectRange,
                _enemyColliders,
                1<<_agentConfig.EnemyLayer
            );
            if (numCollisions > 0)
                SetTarget(new Target(_enemyColliders[0].transform)); // TODO: re-evaluate this "pick first" strategy
            else
            {
                _target = null;
                TargetLost?.Invoke();
            }
        }

        private bool HasTarget() => _target != null;

        private bool EnemyWithinRange()
        {
            DetectEnemy();
            return HasTarget();
        }

        private Status MoveToTarget()
        {
            if (_target == null)
                return Status.Failure;
            if (Vector3.Distance(_target.position, transform.position) <= _agentConfig.AttackRange)
                return Status.Success;

            agent.SetDestination(_target.position);
            return Status.Running;
        }
    }
}
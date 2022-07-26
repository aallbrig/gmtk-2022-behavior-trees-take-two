using System;
using Model;
using Model.AI.BehaviorTrees;
using Model.AI.BehaviorTrees.BuildingBlocks;
using Model.Interfaces;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace MonoBehaviours
{
    public class Grunt : MonoBehaviour, ITrackTargets, IBehaviorTreeProvider
    {
        private static int _maxEnemyColliders = 3;
        public event Action<Target> TargetAcquired;
        public event Action<Target> TargetLost;
        public float thinkRateInSeconds = 1f;
        public NavMeshAgent agent;
        public float detectRange = 10f;
        public float attackRange = 5f;
        public int enemyLayerMask;
        private float _timeOfLastThought;
        private Transform _target;
        private BehaviorTree _brain;
        private readonly Collider[] _enemyColliders = new Collider[_maxEnemyColliders];
        #if UNITY_EDITOR
        private Color _agentGizmoColor;
        #endif

        private void Start()
        {
            if (enemyLayerMask == default) throw new NullReferenceException();
            agent ??= GetComponent<NavMeshAgent>();
            if (agent == null) throw new NullReferenceException();

            _brain = ProvideBehaviorTree();
            // let the brain think instantly
            _timeOfLastThought = Time.time - thinkRateInSeconds;
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
            // draw detect range
            Gizmos.color = _agentGizmoColor;
            Gizmos.DrawWireSphere(transform.position, detectRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
        #endif


        private void Think()
        {
            if (Time.time - _timeOfLastThought > thinkRateInSeconds) _brain.Run();
        }

        public void SetTarget(Target target)
        {
            _target = target.transform ? target.transform : throw new ArgumentNullException(nameof(target));
            TargetAcquired?.Invoke(target);
        }

        public event Action<BehaviorTree> BehaviorTreeProvided;

        private void DetectEnemy()
        {
            var numCollisions = Physics.OverlapSphereNonAlloc(
                transform.position,
                detectRange,
                _enemyColliders,
                1<<enemyLayerMask
            );
            if (numCollisions > 0)
                _target = _enemyColliders[0].transform;
        }

        private bool HasTarget() => _target != null;

        private bool EnemyWithinRange()
        {
            DetectEnemy();
            return HasTarget();
        }

        private Status MoveToTarget()
        {
            if (!_target)
                return Status.Failure;
            if (Vector3.Distance(_target.position, transform.position) <= attackRange)
                return Status.Success;

            agent.SetDestination(_target.position);
            return Status.Running;
        }

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
    }
}
using System;
using Model;
using Model.AI.BehaviorTrees;
using Model.Interfaces;
using Model.Interfaces.BattleSystem;
using MonoBehaviours.BattleSystem;
using MonoBehaviours.Brains;
using MonoBehaviours.Sensors;
using ScriptableObjects.Agent;
using UnityEngine;
using UnityEngine.AI;

namespace MonoBehaviours
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Grunt : MonoBehaviour, ITrackTargets, IMonobehaviourDebugLogger
    {
        public ProximitySensor proximitySensor;
        public WeaponsUser weaponsUser;
        public BehaviorTreeRunner behaviorTreeRunner;
        private static readonly GruntBehaviorTreeProvider BehaviorTreeProvider = new GruntBehaviorTreeProvider();

        public event Action<Target> TargetAcquired;
        public event Action TargetLost;
        public NavMeshAgent agent;
        public AgentConfiguration agentConfiguration;

        private IAgentConfiguration AgentConfig => agentConfiguration;
        private IWeaponsUser WeaponsUser { get; set; }
        [SerializeField] private Transform target;
        [SerializeField] private bool debugEnabled;

        private void Start()
        {
            WeaponsUser ??= weaponsUser;
            WeaponsUser ??= GetComponent<WeaponsUser>();
            if (WeaponsUser == null)
                throw new ArgumentNullException(
                    nameof(WeaponsUser),
                    "A weapons user is required for a grunt to operate"
                );
            agentConfiguration ??= ScriptableObject.CreateInstance<AgentConfiguration>();
            if (proximitySensor)
                ConfigureTargetingSystem();

            // Why shouldn't this always happen?
            if (behaviorTreeRunner.BehaviorTree == null)
                behaviorTreeRunner.SetBehaviorTree(BehaviorTreeProvider.ProvideBehaviorTree(this));

            agent ??= GetComponent<NavMeshAgent>();
            agent.speed = AgentConfig.WalkSpeed;
        }

        private void ConfigureTargetingSystem()
        {
            proximitySensor.EnemySensed += acquired =>
            {
                if (target == null) SetTarget(new Target(acquired.transform));
            };
            proximitySensor.EnemySenseLost += targetLost =>
            {
                if (target == targetLost.transform)
                {
                    target = null;
                    TargetLost?.Invoke();
                }
            };
        }

        public void SetTarget(Target newTarget)
        {
            target = newTarget.Transform ? newTarget.Transform : throw new ArgumentNullException(nameof(newTarget));
            TargetAcquired?.Invoke(newTarget);
        }

        public bool HasTarget()
        {
            DebugLog($"has target? {target != null}");
            return target != null;
        }

        public Status MoveToTarget()
        {
            if (HasTarget() == false)
            {
                DebugLog("No target -- failure");
                return Status.Failure;
            }
            var distanceToTarget = Vector3.Distance(target.position, transform.position);
            DebugLog($"distance to target {distanceToTarget}");
            if (distanceToTarget >= AgentConfig.DetectRange)
            {
                DebugLog("target too far! -- failure");
                agent.SetDestination(transform.position);
                return Status.Failure;
            }
            if (WeaponsUser.Weapon != default && distanceToTarget > WeaponsUser.Weapon.EffectiveRange)
            {
                DebugLog($"moving to target (pos {target.transform.position}) -- running");
                MovingCloserToTarget?.Invoke(target.gameObject);
                agent.SetDestination(target.position);
                return Status.Running;
            }
            if (WeaponsUser.Weapon != default && distanceToTarget <= WeaponsUser.Weapon.EffectiveRange)
            {
                DebugLog("Close enough to target -- success!");
                agent.SetDestination(transform.position);
                return Status.Success;
            }
            DebugLog("Unhandled case -- failure");
            return Status.Failure;
        }

        public event Action<GameObject> MovingCloserToTarget;

        public void DebugLog(string logMessage)
        {
            if (DebugEnabled)
            {
                Debug.Log($"{name} | <Grunt> | {logMessage}");
            }
        }

        public bool DebugEnabled
        {
            get => debugEnabled;
            set => debugEnabled = value;
        }
    }
}
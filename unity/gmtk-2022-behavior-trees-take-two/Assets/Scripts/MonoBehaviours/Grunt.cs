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
    public class Grunt : MonoBehaviour, ITrackTargets
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
        private Transform _target;

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
                if (_target == null) SetTarget(new Target(acquired.transform));
            };
            proximitySensor.EnemySenseLost += targetLost =>
            {
                if (_target == targetLost.transform)
                {
                    _target = null;
                    TargetLost?.Invoke();
                }
            };
        }

        public void SetTarget(Target target)
        {
            _target = target.Transform ? target.Transform : throw new ArgumentNullException(nameof(target));
            TargetAcquired?.Invoke(target);
        }

        public bool HasTarget() => _target != null;

        public Status MoveToTarget()
        {
            if (HasTarget() == false)
                return Status.Failure;
            var distanceToTarget = Vector3.Distance(_target.position, transform.position);
            if (distanceToTarget >= AgentConfig.DetectRange)
            {
                agent.SetDestination(transform.position);
                return Status.Failure;
            }
            if (WeaponsUser.Weapon != default && distanceToTarget > WeaponsUser.Weapon.EffectiveRange)
            {
                MovingCloserToTarget?.Invoke();
                agent.SetDestination(_target.position);
                return Status.Running;
            }
            if (WeaponsUser.Weapon != default && distanceToTarget <= WeaponsUser.Weapon.EffectiveRange)
            {
                agent.SetDestination(transform.position);
                return Status.Success;
            }
            return Status.Failure;
        }

        public event Action MovingCloserToTarget;
    }
}
using System;
using System.Collections.Generic;
using Model.Interfaces;
using ScriptableObjects;
using UnityEngine;

namespace MonoBehaviours.BattleSystem
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class Targeting : MonoBehaviour, ITargetSystem
    {
        public LayerMask friendlyLayerMask;
        public LayerMask enemiesLayerMask;
        public LayerMask neutralLayerMask;

        [SerializeField] public List<BattleAgent> friendlies = new List<BattleAgent>();
        [SerializeField] public List<BattleAgent> enemies = new List<BattleAgent>();
        [SerializeField] public List<BattleAgent> neutrals = new List<BattleAgent>();
        public TargetSystemConfiguration configuration;

        private Collider _collider;
        private Rigidbody _rigidbody;

        private void Start()
        {
            EnsureComponentReady();
            ApplyComponentExpectations();
        }

        private void EnsureComponentReady()
        {
            configuration ??= ScriptableObject.CreateInstance<TargetSystemConfiguration>();
            _collider = GetComponent<Collider>();

            _rigidbody = GetComponent<Rigidbody>();
        }

        [ContextMenu("Apply Component Expectation")]
        public void ApplyComponentExpectations()
        {
            EnsureComponentReady();

            _collider.isTrigger = true;
            if (_collider is SphereCollider sphereCollider)
                sphereCollider.radius = Configuration.DetectRadius;
            _rigidbody.useGravity = false;
        }

        private void AcquireNewTarget(BattleAgent battleAgent, List<BattleAgent> currentTrackedTargets)
        {
            if (currentTrackedTargets.Contains(battleAgent) == true) return;

            currentTrackedTargets.Add(battleAgent);
            TargetAcquired?.Invoke(new TargetAcquired(battleAgent, currentTrackedTargets));
        }

        private void UnacquireTrackedTarget(BattleAgent battleAgent, List<BattleAgent> currentTrackedTargets)
        {
            if (currentTrackedTargets.Contains(battleAgent) == false) return;
    
            currentTrackedTargets.Remove(battleAgent);
            TargetLost?.Invoke(new TrackedTargetLost(battleAgent, currentTrackedTargets));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<BattleAgent>(out var battleAgent)) return;

            if (GameObjectInLayerMask(other.gameObject, friendlyLayerMask))
                AcquireNewTarget(battleAgent, friendlies);
            if (GameObjectInLayerMask(other.gameObject, enemiesLayerMask))
                AcquireNewTarget(battleAgent, enemies);
            if (GameObjectInLayerMask(other.gameObject, neutralLayerMask))
                AcquireNewTarget(battleAgent, neutrals);
        }

        private static bool GameObjectInLayerMask(GameObject otherGameObject, LayerMask layerMask) =>
            layerMask == (layerMask | (1 << otherGameObject.layer));

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<BattleAgent>(out var battleAgent)) return;

            if (friendlies.Contains(battleAgent) && GameObjectInLayerMask(other.gameObject, friendlyLayerMask))
                UnacquireTrackedTarget(battleAgent, friendlies);
            if (enemies.Contains(battleAgent) && GameObjectInLayerMask(other.gameObject, enemiesLayerMask))
                UnacquireTrackedTarget(battleAgent, enemies);
            if (neutrals.Contains(battleAgent) && GameObjectInLayerMask(other.gameObject, neutralLayerMask))
                UnacquireTrackedTarget(battleAgent, neutrals);
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos() {}

        private void OnDrawGizmosSelected() {}
        #endif

        public event Action<TargetAcquired> TargetAcquired;

        public event Action<TrackedTargetLost> TargetLost;

        public ITargetingSystemConfiguration Configuration => configuration;

        public IEnumerable<IGameAgent> Friendlies => friendlies;

        public IEnumerable<IGameAgent> Enemies => enemies;

        public IEnumerable<IGameAgent> Neutrals => neutrals;
    }
}
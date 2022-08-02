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
            {
                sphereCollider.radius = Configuration.DetectRadius;
            }
            _rigidbody.useGravity = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<BattleAgent>(out var battleAgent)) return;
    
            if (friendlyLayerMask == 1 << other.gameObject.layer)
                friendlies.Add(battleAgent);
            if (enemiesLayerMask == 1 << other.gameObject.layer)
                enemies.Add(battleAgent);
            if (neutralLayerMask == 1 << other.gameObject.layer)
                neutrals.Add(battleAgent);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<BattleAgent>(out var battleAgent)) return;

            if (friendlies.Contains(battleAgent) && friendlyLayerMask == 1 << other.gameObject.layer)
                friendlies.Remove(battleAgent);
            if (enemies.Contains(battleAgent) &&enemiesLayerMask == 1 << other.gameObject.layer)
                enemies.Remove(battleAgent);
            if (neutrals.Contains(battleAgent) && neutralLayerMask == 1 << other.gameObject.layer)
                neutrals.Remove(battleAgent);
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            
        }

        private void OnDrawGizmosSelected()
        {
            
        }
        #endif

        public ITargetingSystemConfiguration Configuration => configuration;

        public IEnumerable<IGameAgent> Friendlies => friendlies;

        public IEnumerable<IGameAgent> Enemies => enemies;

        public IEnumerable<IGameAgent> Neutrals => neutrals;
    }
}
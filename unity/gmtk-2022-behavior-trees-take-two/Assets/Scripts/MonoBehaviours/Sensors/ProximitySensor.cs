using System;
using System.Collections.Generic;
using Model.Interfaces.Sensors;
using UnityEngine;

namespace MonoBehaviours.Sensors
{
    [RequireComponent(typeof(Collider))]
    public class ProximitySensor : MonoBehaviour, IProximitySensor
    {
        public IProximitySensorConfiguration Configuration;
        private Collider _collider;

        [SerializeField] public List<GameObject> Friendlies { get; } = new List<GameObject>();
        [SerializeField] public List<GameObject> Enemies { get; } = new List<GameObject>();
        [SerializeField] public List<GameObject> CoverPoints { get; } = new List<GameObject>();

        private void Start()
        {
            _collider = GetComponent<Collider>();
            ConfigureCollider();
        }

        private void ConfigureCollider()
        {
            _collider.isTrigger = true;
            if (_collider is SphereCollider sphereCollider)
            {
                sphereCollider.radius = Configuration.Range / 2;
            }
        }   

        public void Sense()
        {
            SenseForFriendlies();
            SenseForEnemies();
            SenseForCoverPoints();
        }

        public void SenseForCoverPoints()
        {
            CoverPoints.Clear();

            foreach (var otherCollider in Physics.OverlapSphere(gameObject.transform.position, Configuration.Range, Configuration.CoverLayerMask))
                if (otherCollider != _collider) CoverPoints.Add(otherCollider.gameObject);
        }

        public void SenseForEnemies()
        {
            Enemies.Clear();

            foreach (var otherCollider in Physics.OverlapSphere(gameObject.transform.position, Configuration.Range, Configuration.EnemyLayerMask))
                if (otherCollider != _collider) Enemies.Add(otherCollider.gameObject);
        }

        public void SenseForFriendlies()
        {
            Friendlies.Clear();

            foreach (var otherCollider in Physics.OverlapSphere(gameObject.transform.position, Configuration.Range, Configuration.FriendlyLayerMask))
                if (otherCollider != _collider) Friendlies.Add(otherCollider.gameObject);
        }
    }
}
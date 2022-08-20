using System;
using System.Collections.Generic;
using Model.Interfaces.Sensors;
using ScriptableObjects;
using UnityEngine;

namespace MonoBehaviours.Sensors
{
    [RequireComponent(typeof(Collider))]
    public class ProximitySensor : MonoBehaviour, IProximitySensor
    {
        public event Action<GameObject> EnemySensed;
        public event Action<GameObject> EnemySenseLost;

        public IProximitySensorConfiguration Configuration;
        public ProximitySensorConfiguration sensorConfig;
        [SerializeField] private Collider sensorTrigger;
        [SerializeField] private List<GameObject> friendlies = new List<GameObject>();
        [SerializeField] private List<GameObject> enemies = new List<GameObject>();
        [SerializeField] private List<GameObject> cover = new List<GameObject>();

        public List<GameObject> Friendlies => friendlies;
        public List<GameObject> Enemies => enemies;
        public List<GameObject> CoverPoints => cover;

        private void Start()
        {
            sensorTrigger ??= GetComponent<Collider>();
            sensorTrigger ??= GetComponent<SphereCollider>();
            if (Configuration == null)
            {
                sensorConfig ??= ScriptableObject.CreateInstance<ProximitySensorConfiguration>();
                Configuration = sensorConfig;
            }

            ConfigureCollider();
        }

        private void Update()
        {
            Sense();
        }

        private void ConfigureCollider()
        {
            sensorTrigger.isTrigger = true;
            if (sensorTrigger is SphereCollider sphereCollider)
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
                if (otherCollider != sensorTrigger && !CoverPoints.Contains(otherCollider.gameObject)) CoverPoints.Add(otherCollider.gameObject);
        }

        public void SenseForEnemies()
        {
            Enemies.Clear();

            foreach (var otherCollider in Physics.OverlapSphere(gameObject.transform.position, Configuration.Range, Configuration.EnemyLayerMask))
                if (otherCollider != sensorTrigger && !Enemies.Contains(otherCollider.gameObject))
                {
                    Enemies.Add(otherCollider.gameObject);
                    EnemySensed?.Invoke(otherCollider.gameObject);
                }
        }

        public void SenseForFriendlies()
        {
            Friendlies.Clear();

            foreach (var otherCollider in Physics.OverlapSphere(gameObject.transform.position, Configuration.Range, Configuration.FriendlyLayerMask))
                if (otherCollider != sensorTrigger && !Friendlies.Contains(otherCollider.gameObject)) Friendlies.Add(otherCollider.gameObject);
        }
    }
}
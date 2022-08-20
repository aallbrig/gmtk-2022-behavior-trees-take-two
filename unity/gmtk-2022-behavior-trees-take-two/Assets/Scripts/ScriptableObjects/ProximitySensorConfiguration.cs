using Model.Interfaces.Sensors;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "new proximity sensor config", menuName = "Game/new proximity sensor config SO", order = 0)]
    public class ProximitySensorConfiguration : ScriptableObject, IProximitySensorConfiguration
    {
        public float sensorRange = 5f;
        public LayerMask friendlyLayerMask;
        public LayerMask enemyLayerMask;
        public LayerMask coverLayerMask;

        public LayerMask FriendlyLayerMask => friendlyLayerMask;
        public LayerMask EnemyLayerMask => enemyLayerMask;
        public LayerMask CoverLayerMask => coverLayerMask;
        public float Range => sensorRange;
    }
}
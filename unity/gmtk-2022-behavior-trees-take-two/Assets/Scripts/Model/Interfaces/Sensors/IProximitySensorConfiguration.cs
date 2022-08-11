using UnityEngine;

namespace Model.Interfaces.Sensors
{
    public interface IProximitySensorConfiguration
    {
        public LayerMask FriendlyLayerMask { get; }
        public LayerMask EnemyLayerMask { get; }
        public LayerMask CoverLayerMask { get; }
        public float Range { get; }
    }
}
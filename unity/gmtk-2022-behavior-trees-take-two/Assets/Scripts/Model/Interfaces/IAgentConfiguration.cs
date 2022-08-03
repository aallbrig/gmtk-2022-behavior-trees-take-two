using UnityEngine;

namespace Model.Interfaces
{
    public interface IAgentLocomotionConfiguration
    {
        public float WalkSpeed { get; }
        public float RunSpeed { get; }
    }

    public interface IAgentCombatConfiguration
    {
        public float DetectRange { get; }
        public float AttackRange { get; }
        public LayerMask EnemyLayerMask { get; }
        public LayerMask FriendlyLayerMask { get; }
        public LayerMask NeutralsLayerMask { get; }
    }

    public interface IAgentConfiguration : IAgentLocomotionConfiguration, IAgentCombatConfiguration
    {
        public string Name { get; }
        public float ThinkRate { get; }
    }
}
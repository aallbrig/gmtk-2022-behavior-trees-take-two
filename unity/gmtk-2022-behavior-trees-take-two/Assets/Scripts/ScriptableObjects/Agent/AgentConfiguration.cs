using Model.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Agent
{
    [CreateAssetMenu(fileName = "new agent config", menuName = "GAME/Agent Config", order = 0)]
    public class AgentConfiguration : ScriptableObject, IAgentConfiguration
    {
        public string agentConfigurationName = "new agent config";
        public int enemyLayerNumber = 7;
        public float walkSpeed = 2f;
        public float runSpeed = 5f;
        public float detectRange = 10f;
        public float attackRange = 5f;
        public float thinkRateInSeconds = 1f;

        public float WalkSpeed => walkSpeed;
        public float RunSpeed => runSpeed;
        public string Name => agentConfigurationName;
        public float ThinkRate => thinkRateInSeconds;
        public float DetectRange => detectRange;
        public float AttackRange => attackRange;
        public int EnemyLayer => enemyLayerNumber;
    }
}
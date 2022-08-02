using System.Collections.Generic;
using UnityEngine;

namespace MonoBehaviours
{
    [RequireComponent(typeof(Collider))]
    public class Targeting : MonoBehaviour
    {
        public BattleAgent battleAgent;
        [SerializeField] public List<BattleAgent> Friends = new List<BattleAgent>();
        [SerializeField] public List<BattleAgent> Targets = new List<BattleAgent>();

        private void Start()
        {
            battleAgent ??= gameObject.GetComponent<BattleAgent>();
        }
        private void OnTriggerEnter(Collider other)
        {
            other.TryGetComponent<BattleAgent>(out var otherBattleAgent);
            if (!otherBattleAgent) return;

            var isFriend = battleAgent.IFF(otherBattleAgent);
            if (isFriend) Friends.Add(otherBattleAgent);
            else Targets.Add(otherBattleAgent);
        }

        private void OnTriggerExit(Collider other)
        {
            other.TryGetComponent<BattleAgent>(out var otherBattleAgent);
            if (!otherBattleAgent) return;

            var isFriend = battleAgent.IFF(otherBattleAgent);
            if (isFriend) Friends.Remove(otherBattleAgent);
            else Targets.Remove(otherBattleAgent);
        }
    }
}
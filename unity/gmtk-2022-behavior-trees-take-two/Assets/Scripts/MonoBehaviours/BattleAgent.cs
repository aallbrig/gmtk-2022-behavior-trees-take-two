using Model.Interfaces;
using UnityEngine;

namespace MonoBehaviours
{
    public class BattleAgent : MonoBehaviour, IGameAgent
    {
        public GameObject GameObject => gameObject;
    }
}
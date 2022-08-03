using System;
using Model.Interfaces;
using UnityEngine;

namespace MonoBehaviours
{
    public class BattleAgent : MonoBehaviour, IGameAgent
    {
        private Guid _id;

        private void Start()
        {
            _id = new Guid();
        }

        public Guid ID => _id;
        public GameObject GameObject => gameObject;
    }
}
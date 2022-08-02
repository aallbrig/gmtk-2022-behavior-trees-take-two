using System;
using MonoBehaviours.BattleSystem;
using UnityEngine;

namespace MonoBehaviours
{
    [RequireComponent(typeof(CharacterController))]
    public class MasterChief : MonoBehaviour, IKnowAboutCamera
    {
        public Targeting targetingSystem;
        private CharacterController _characterController;
        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
        }

        public Camera MainCamera => Camera.main;
    }

}
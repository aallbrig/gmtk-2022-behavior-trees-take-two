using System;
using Model.Interfaces;
using MonoBehaviours.Brains;
using MonoBehaviours.Controllers;
using MonoBehaviours.Sensors;
using ScriptableObjects.Agent;
using UnityEngine;

namespace MonoBehaviours
{
    [RequireComponent(typeof(CharacterController))]
    public class MasterChief : MonoBehaviour, IKnowAboutCamera, IMonobehaviourDebugLogger
    {
        public BehaviorTreeRunner behaviorTreeRunner;
        private static readonly MasterChiefBehaviorTreeProvider BehaviorTreeProvider = new MasterChiefBehaviorTreeProvider();
        public ProximitySensor proximitySensor;
        public PlayerController playerController;
        private CharacterController _characterController;
        [SerializeField] private bool debugEnabled;
        private GameObject _currentTarget;
        private bool _isProcessingUserInput = false;
        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            var behaviorTree = BehaviorTreeProvider.ProvideBehaviorTree(this);
            behaviorTreeRunner ??= GetComponent<BehaviorTreeRunner>();
            behaviorTreeRunner.SetBehaviorTree(behaviorTree);
            playerController ??= GetComponent<PlayerController>();
            proximitySensor ??= GetComponent<ProximitySensor>();
            if (playerController == null)
            {
                DebugLog("Player controller missing");
                throw new ArgumentNullException();
            }
            else
            {
                playerController.UserInputStart += () =>
                {
                    _isProcessingUserInput = true;
                };
                playerController.UserInputEnd += () =>
                {
                    _isProcessingUserInput = false;
                };
            }
            if (proximitySensor == null)
            {
                DebugLog("Proximity sensor missing");
                throw new ArgumentNullException();
            }
            else
            {
                proximitySensor.EnemySensed += go =>
                {
                    if (_currentTarget == null)
                    {
                        _currentTarget = go;
                        DebugLog($"target acquired: {_currentTarget}");
                        TargetAcquired?.Invoke(_currentTarget);
                    }
                };
            }
        }

        public bool ProcessingUserInput() => _isProcessingUserInput;
        public bool HasCurrentTarget () => _currentTarget != null;
        public Camera MainCamera => Camera.main;
        public event Action<GameObject> TargetAcquired;
        public event Action<GameObject> ShootingTarget;

        public void DebugLog(string logMessage)
        {
            if (DebugEnabled)
            {
                Debug.Log($"{name} | <Master Chief> | {logMessage}");
            }
        }

        public bool DebugEnabled
        {
            get => debugEnabled;
            set => debugEnabled = value;
        }

        public void ShootTarget()
        {
            if (_characterController)
            {
                DebugLog($"Shooting current target {_currentTarget}");
                ShootingTarget?.Invoke(_currentTarget);
            }
        }
    }

}
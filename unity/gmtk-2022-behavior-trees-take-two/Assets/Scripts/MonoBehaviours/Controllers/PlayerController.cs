using System;
using Generated;
using Model.Interfaces;
using Model.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MonoBehaviours.Controllers
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour, ILocomotion, IMonobehaviourDebugLogger
    {
        public Camera perspectiveCamera;
        public float movementSpeed = 4;
        public float rotationSpeed = 15;
        private CharacterController _characterController;
        private Camera _perspectiveCamera;
        private PlayerControls _controls;
        [Header("Runtime Info")] public Vector2 pointerPerformedPosition;
        [SerializeField] private Vector2 pointerStartPosition;
        [SerializeField] private Vector2 pointerEndPosition;
        [SerializeField] private Vector3 desiredWorldDirection = Vector3.zero;
        [SerializeField] private bool isGrounded = true;
        [SerializeField] private Vector3 velocity;
        [SerializeField] private bool debugEnabled;
        private bool touchInputing = false;

        private void Awake() => _controls = new PlayerControls();
        private void OnEnable() => _controls.Enable();
        private void OnDisable() => _controls.Disable();
        private void Start()
        {
            _perspectiveCamera ??= perspectiveCamera;
            _perspectiveCamera ??= Camera.main;
            _characterController ??= GetComponent<CharacterController>();
            _controls.MasterChief.PointerButtonPress.started += HandlePointerPressStart;
            _controls.MasterChief.PointerButtonPress.canceled += HandlePointerPressExit;
            _controls.MasterChief.PointerPosition.performed += SyncPointerPosition;
            _controls.MasterChief.MoveJoystick.started += HandleMoveJoystickStart;
            _controls.MasterChief.MoveJoystick.canceled += HandleMoveJoystickEnd;
            _controls.MasterChief.MoveJoystick.performed += HandleMoveJoystickPerformed;
        }
        private void HandleMoveJoystickStart(InputAction.CallbackContext ctx)
        {
            var value = ctx.ReadValue<Vector2>();
            CalculateRelativeMovement(value);
            DebugLog($" <JoystickMove> | start {value}");
            UserInputStart?.Invoke();
        }
        private void HandleMoveJoystickPerformed(InputAction.CallbackContext ctx)
        {
            var value = ctx.ReadValue<Vector2>();
            CalculateRelativeMovement(value);
            DebugLog($" <JoystickMove> | performed {value}");
        }
        private void HandleMoveJoystickEnd(InputAction.CallbackContext ctx)
        {
            var value = ctx.ReadValue<Vector2>();
            CalculateRelativeMovement(Vector2.zero);
            DebugLog($" <JoystickMove> | end");
            UserInputEnd?.Invoke();
        }
        private void Update()
        {
            isGrounded = _characterController.isGrounded;
            velocity = _characterController.velocity;
            if (DesiredDirectionInWorld != Vector3.zero)
            {
                var lookDirection = Quaternion.LookRotation(DesiredDirectionInWorld);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookDirection, rotationSpeed * Time.deltaTime);
            }
            _characterController.Move(((DesiredDirectionInWorld * movementSpeed) + new Vector3(0, -9.81f, 0)) * Time.deltaTime);
        }

        private void HandlePointerPressStart(InputAction.CallbackContext ctx)
        {
            touchInputing = true;
            pointerStartPosition = pointerPerformedPosition;
            DebugLog($" <TouchMove> | (start pos: {pointerStartPosition})");
            UserInputStart?.Invoke();
        }
        private void SyncPointerPosition(InputAction.CallbackContext ctx)
        {
            pointerPerformedPosition = ctx.ReadValue<Vector2>();
            if (touchInputing)
            {
                var inputDirection = (pointerPerformedPosition - pointerStartPosition).normalized;
                DebugLog($" <TouchMove> | performed (inputDir {inputDirection}, start pos: {pointerStartPosition} end pos: {pointerPerformedPosition})");
                CalculateRelativeMovement(inputDirection);
            }
        }

        private void HandlePointerPressExit(InputAction.CallbackContext ctx)
        {
            touchInputing = false;
            CalculateRelativeMovement(Vector2.zero);
            UserInputEnd?.Invoke();
        }

        private void CalculateRelativeMovement(Vector2 inputDirection)
        {
            if (inputDirection == Vector2.zero)
            {
                DesiredDirectionInWorld = Vector3.zero;
                return;
            }

            var cameraTransform = _perspectiveCamera.transform;
            var right = cameraTransform.right;
            var forward = Vector3.Cross(Vector3.right, Vector3.up);
            forward.y = 0;
            right.y = 0;
            forward = forward.normalized;
            right = right.normalized;
            var relativeForward= inputDirection.y * forward;
            var relativeRight = inputDirection.x * right;
            DesiredDirectionInWorld = relativeForward + relativeRight;
        }

        public Vector3 DesiredDirectionInWorld
        {
            get => desiredWorldDirection;
            private set => desiredWorldDirection = value;
        }

        public event Action UserInputStart;
        public event Action UserInputEnd;

        public void DebugLog(string logMessage)
        {
            if (DebugEnabled) Debug.Log($"{name} | <PlayerController> | {logMessage}");
        }

        public bool DebugEnabled
        {
            get => debugEnabled;
            set => debugEnabled = value;
        }
    }
}
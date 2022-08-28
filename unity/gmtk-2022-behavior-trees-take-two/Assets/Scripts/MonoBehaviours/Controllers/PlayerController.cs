using Generated;
using Model.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MonoBehaviours.Controllers
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour, ILocomotion
    {
        public Camera perspectiveCamera;
        public float movementSpeed = 4;
        private CharacterController _characterController;
        private Camera _perspectiveCamera;
        private PlayerControls _controls;
        [Header("Runtime Info")]
        [SerializeField] private Vector2 pointerStartPosition;
        [SerializeField] private Vector2 pointerEndPosition;
        [SerializeField] private Vector3 desiredDirectionInWorld = Vector3.zero;
        [SerializeField] private bool isGrounded = true;

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
        }
        private void Update()
        {
            isGrounded = _characterController.isGrounded;
            if (DesiredDirectionInWorld != Vector3.zero)
            {
                _characterController.Move(DesiredDirectionInWorld * movementSpeed * Time.deltaTime);
                // _characterController.transform.Rotate();
                transform.rotation = Quaternion.LookRotation(DesiredDirectionInWorld);
            }
        }

        private void HandlePointerPressStart(InputAction.CallbackContext ctx)
        {
            pointerStartPosition = _controls.MasterChief.PointerPosition.ReadValue<Vector2>();
        }

        private void HandlePointerPressExit(InputAction.CallbackContext ctx)
        {
            pointerEndPosition = _controls.MasterChief.PointerPosition.ReadValue<Vector2>();
            CalculateRelativeMovement();
        }
        private void CalculateRelativeMovement()
        {
            var inputDirection = (pointerEndPosition - pointerStartPosition).normalized;
            var cameraTransform = _perspectiveCamera.transform;
            var forward = cameraTransform.forward;
            forward.y = 0;
            var right = cameraTransform.right;
            right.y = 0;
            var relativeForward= inputDirection.y * forward;
            relativeForward.y = 0;
            var relativeRight = inputDirection.x * right;
            relativeRight.y = 0;
            DesiredDirectionInWorld = (relativeForward + relativeRight).normalized;
        }

        public Vector3 DesiredDirectionInWorld
        {
            get => desiredDirectionInWorld;
            private set => desiredDirectionInWorld = value;
        } 
    }
}
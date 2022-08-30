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
        public float rotationSpeed = 15;
        public bool isInputing = false;
        private CharacterController _characterController;
        private Camera _perspectiveCamera;
        private PlayerControls _controls;
        [Header("Runtime Info")] public Vector2 pointerPerformedPosition;
        [SerializeField] private Vector2 pointerStartPosition;
        [SerializeField] private Vector2 pointerEndPosition;
        [SerializeField] private Vector3 desiredWorldDirection = Vector3.zero;
        [SerializeField] private bool isGrounded = true;
        [SerializeField] private Vector3 velocity;

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
            _controls.MasterChief.PointerPosition.performed += context =>
            {
                pointerPerformedPosition = context.ReadValue<Vector2>();
                CalculateRelativeMovement();
            };
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
            pointerStartPosition = _controls.MasterChief.PointerPosition.ReadValue<Vector2>();
            isInputing = true;
        }

        private void HandlePointerPressExit(InputAction.CallbackContext ctx)
        {
            isInputing = false;
            DesiredDirectionInWorld = Vector3.zero;
        }

        private void CalculateRelativeMovement()
        {
            if (isInputing == false) return;

            var inputDirection = (pointerPerformedPosition - pointerStartPosition).normalized;
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
    }
}
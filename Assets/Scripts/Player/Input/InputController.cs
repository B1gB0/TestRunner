using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Input
{
    public class InputController : MonoBehaviour
    {
        private const float MinMagnitude = 0.01f;
        private const float MinValue = 0f;

        [Header("Action Locks")]
        [SerializeField] private bool _isMovementLocked;

        private InputSystem _inputSystem;
        private Joystick _joystick;
        
        public event Action OnMoveButtonsPressed;
        public event Action OnUnlockController;

        public Vector2 MoveDirection { get; private set; }
        public bool IsMoveInputPerformed { get; private set; }

        private void Awake()
        {
            _inputSystem = new InputSystem();
        }

        private void OnEnable()
        {
            _inputSystem.PLayer.Enable();

            _inputSystem.PLayer.Move.performed += OnMove;
            _inputSystem.PLayer.Move.canceled += OnMove;
        }

        private void OnDisable()
        {
            _inputSystem.PLayer.Move.performed -= OnMove;
            _inputSystem.PLayer.Move.canceled -= OnMove;

            _inputSystem.PLayer.Disable();
        }

        private void OnDestroy()
        {
            if (_joystick != null) _joystick.OnInputHandled -= OnMoveWithJoystick;
        }

        public void GetJoystickWithAttackButton(Joystick joystick)
        {
            _joystick = joystick;
            _joystick.OnInputHandled += OnMoveWithJoystick;
        }

        public void LockPlayerMovement()
        {
            Debug.Log("LockPlayerMovement");
            _isMovementLocked = true;
        }
        
        public void UnlockPlayerMovement()
        {
            Debug.Log("UnlockPlayerMovement");
            _isMovementLocked = false;
            
            OnUnlockController?.Invoke();
        }

        private void OnMoveWithJoystick()
        {
            if (_isMovementLocked)
            {
                MoveDirection = Vector2.zero;
                IsMoveInputPerformed = false;
                return;
            }
            
            MoveDirection = _joystick.Direction;
            IsMoveInputPerformed = MoveDirection.sqrMagnitude > MinMagnitude;
            if (IsMoveInputPerformed) OnMoveButtonsPressed?.Invoke();
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                MoveDirection = context.ReadValue<Vector2>();
                IsMoveInputPerformed = MoveDirection.sqrMagnitude > MinMagnitude;
            }
            else if (context.canceled || _isMovementLocked)
            {
                MoveDirection = Vector2.zero;
                IsMoveInputPerformed = false;
            }

            if (IsMoveInputPerformed) OnMoveButtonsPressed?.Invoke();
        }
    }
}
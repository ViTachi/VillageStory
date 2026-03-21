using Gameplay;
using Input;
using UnityEngine;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public class PlayerMovementService : IPlayerMovementService
    {
        [Inject] private IPlayerController _playerController;
        private PlayerInput _playerInput;

        public void Initialize()
        {
            _playerInput = new PlayerInput();

            _playerInput.Player.Move.performed += Move;
            _playerInput.Player.Jump.performed += Jump;
            _playerInput.Player.Crouch.performed += OnCrouch;
        }

        public void Enable() =>
            _playerInput.Enable();

        public void Disable() =>
            _playerInput.Disable();

        public void Dispose()
        {
            _playerInput.Player.Move.performed -= Move;
            _playerInput.Player.Jump.performed -= Jump;
            _playerInput.Player.Crouch.performed -= OnCrouch;
        }

        private void Move(CallbackContext callbackContext)
        {
            Vector3 inputVector = callbackContext.ReadValue<Vector2>();
            _playerController.Move(new Vector3(inputVector.x, 0f, inputVector.y));
        }

        private void Jump(CallbackContext callbackContext)
        {
            if (callbackContext.ReadValueAsButton())
            {
                _playerController.Jump();
            }
        }

        private void OnCrouch(CallbackContext callbackContext)
        {
            if (callbackContext.started)
            {
                _playerController.SetCrouch(true);
            }
            if (callbackContext.canceled)
            {
                _playerController.SetCrouch(false);
            }
        }
    }
}
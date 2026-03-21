using Gameplay;
using Input;
using UnityEngine;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public class PlayerMovementService : IPlayerMovementService
    {
        [Inject] private IPlayerController playerController;
        private PlayerInput playerInput;

        public void Initialize()
        {
            playerInput = new PlayerInput();

            playerInput.Player.Move.performed += Move;
            playerInput.Player.Jump.performed += Jump;
            playerInput.Player.Crouch.performed += OnCrouch;

            Debug.Log("Initialize");
        }

        public void Enable() =>
            playerInput.Enable();

        public void Disable() =>
            playerInput.Disable();

        public void Dispose()
        {
            playerInput.Player.Move.performed -= Move;
            playerInput.Player.Jump.performed -= Jump;
            playerInput.Player.Crouch.performed -= OnCrouch;
        }

        private void Move(CallbackContext callbackContext)
        {
            Vector3 inputVector = callbackContext.ReadValue<Vector2>();
            playerController.Move(new Vector3(inputVector.x, 0f, inputVector.y));
        }

        private void Jump(CallbackContext callbackContext)
        {
            if (callbackContext.ReadValueAsButton())
            {
                playerController.Jump();
            }
        }

        private void OnCrouch(CallbackContext callbackContext)
        {
            if (callbackContext.started)
            {
                playerController.SetCrouch(true);
            }
            if (callbackContext.canceled)
            {
                playerController.SetCrouch(false);
            }
        }
    }
}
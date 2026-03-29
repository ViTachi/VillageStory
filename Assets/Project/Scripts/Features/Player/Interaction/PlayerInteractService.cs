using System;
using Input;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public class PlayerInteractService : IPlayerInteractService
    {
        [Inject] private IPointerDetectionService _pointerDetectionService;
        [Inject] private PlayerInput _playerInput;

        public void Initialize()
        {
            _playerInput.Player.Interact.started += TryInteract;
        }

        public void Cleanup()
        {
            _playerInput.Player.Interact.started -= TryInteract;
        }

        private void TryInteract(CallbackContext callbackContext)
        {
            if (_pointerDetectionService.ItemInView == null) return;

            if (_pointerDetectionService.ItemInView.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact();
            }
        }
    }
}
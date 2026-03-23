using System;
using Input;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public class PlayerInteractService : IPlayerInteractService, IInitializable, IDisposable
    {
        [Inject] private IPointerDetectionService pointerDetectionService;
        [Inject] private PlayerInput playerInput;

        public void Initialize()
        {
            playerInput.Player.Interact.started += TryInteract;
        }

        public void Dispose()
        {
            playerInput.Player.Interact.started -= TryInteract;
        }

        private void TryInteract(CallbackContext callbackContext)
        {
            if (pointerDetectionService.ItemInView == null) return;

            if (pointerDetectionService.ItemInView.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact();
            }
        }
    }
}
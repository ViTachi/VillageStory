using System;
using Input;
using UI;
using UI.PlayerHud;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public class NpcDialogueService : INpcDialogueService
    {
        [Inject] private PlayerInput _playerInput;
        [Inject] private IPointerDetectionService _pointerDetectionService;
        [Inject] private IUIService _uIService;

        public event Action OnShowDialogue;
        public event Action OnHideDialogue;

        private bool _dialogueActive;
        private IDialoguePartner _currentPartner;
        private PlayerHud _playerHud;

        public void Initialize()
        {
            _playerHud = _uIService.GetScreen<PlayerHud, object, object>();
            _playerInput.Player.Interact.started += TryDialogueInteract;
            _playerInput.Player.Use.started += HideDialogueFromUse;
        }

        public void Cleanup()
        {
            _playerInput.Player.Interact.started -= TryDialogueInteract;
            _playerInput.Player.Use.started -= HideDialogueFromUse;
        }

        private void TryDialogueInteract(CallbackContext _)
        {
            if (_dialogueActive)
            {
                HideDialogue();
                return;
            }

            if (_pointerDetectionService.ItemInView == null) return;

            if (_pointerDetectionService.ItemInView.TryGetComponent(out IDialoguePartner partner))
            {
                ShowDialogue(partner);
            }
        }

        private void HideDialogueFromUse(CallbackContext _)
        {
            if (_dialogueActive)
                HideDialogue();
        }

        private void ShowDialogue(IDialoguePartner partner)
        {
            _dialogueActive = true;
            _currentPartner = partner;
            partner.BeginDialogue();
            _pointerDetectionService.SetActive(false);
            _playerHud.KeyHintInfoView.ShowHintText("[E] - Close");
            OnShowDialogue?.Invoke();
        }

        private void HideDialogue()
        {
            if (!_dialogueActive) return;

            _dialogueActive = false;
            _currentPartner?.EndDialogue();
            _currentPartner = null;
            _playerHud.KeyHintInfoView.HideHintText();
            _pointerDetectionService.SetActive(true);
            OnHideDialogue?.Invoke();
        }
    }
}

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
            _playerInput.Player.Use.started += TryAdvanceDialogue;
            _playerInput.Player.Jump.started += TryAdvanceDialogue;
        }

        public void Cleanup()
        {
            _playerInput.Player.Interact.started -= TryDialogueInteract;
            _playerInput.Player.Use.started -= TryAdvanceDialogue;
            _playerInput.Player.Jump.started -= TryAdvanceDialogue;
        }

        private void TryDialogueInteract(CallbackContext _)
        {
            if (_dialogueActive)
                return;

            if (_pointerDetectionService.ItemInView == null) return;

            if (_pointerDetectionService.ItemInView.TryGetComponent(out IDialoguePartner partner))
            {
                ShowDialogue(partner);
            }
        }

        private void TryAdvanceDialogue(CallbackContext _)
        {
            if (!_dialogueActive) return;

            if (_playerHud.DialogueView.TryAdvance())
                HideDialogue();
        }

        private void ShowDialogue(IDialoguePartner partner)
        {
            _dialogueActive = true;
            _currentPartner = partner;
            partner.BeginDialogue();
            _playerHud.DialogueView.ShowDialogue(partner.GetDialogueData());
            _pointerDetectionService.SetActive(false);
            OnShowDialogue?.Invoke();
        }

        private void HideDialogue()
        {
            if (!_dialogueActive) return;

            _dialogueActive = false;
            _playerHud.DialogueView.Hide();
            _currentPartner?.EndDialogue();
            _currentPartner = null;
            _pointerDetectionService.SetActive(true);
            OnHideDialogue?.Invoke();
        }
    }
}

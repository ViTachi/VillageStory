using System;
using Input;
using UI;
using UI.PlayerHud;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public class LevelNotesService : ILevelNotesService
    {
        [Inject] private PlayerInput _playerInput;
        [Inject] private IPointerDetectionService _pointerDetectionService;
        [Inject] private IUIService _uIService;

        public event Action OnShowNote;
        public event Action OnHideNote;

        private bool _noteShowed = false;
        private PlayerHud _playerHud;

        public void Initialize()
        {
            _playerHud = _uIService.GetScreen<PlayerHud, object, object>();
            _playerInput.Player.Interact.started += TryReadNote;
            _playerInput.Player.Use.started += HideNote;
        }

        public void Cleanup()
        {
            _playerInput.Player.Interact.started -= TryReadNote;
            _playerInput.Player.Use.started -= HideNote;
        }

        private void TryReadNote(CallbackContext callbackContext)
        {
            if (_noteShowed)
            {
                HideNote();
                return;
            }

            if (_pointerDetectionService.ItemInView == null) return;

            if (_pointerDetectionService.ItemInView.TryGetComponent(out ILevelNote levelNote))
            {
                ShowNote(levelNote);
            }
        }

        private void HideNote(CallbackContext callbackContext)
        {
            if (_noteShowed)
                HideNote();
        }

        private void ShowNote(ILevelNote levelNote)
        {
            _noteShowed = true;
            _playerHud.NoteView.ShowNote(levelNote.Text);
            _pointerDetectionService.SetActive(false);
            _playerHud.KeyHintInfoView.ShowHintText("[E] - Close");
            OnShowNote?.Invoke();
        }

        private void HideNote()
        {
            _noteShowed = false;
            _playerHud.NoteView.Hide();
            _playerHud.KeyHintInfoView.HideHintText();
            _pointerDetectionService.SetActive(true);
            OnHideNote?.Invoke();
        }
    }
}


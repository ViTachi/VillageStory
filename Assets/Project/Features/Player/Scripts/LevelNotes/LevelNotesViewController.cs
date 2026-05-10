using UI;
using UI.PlayerHud;
using UnityEngine;
using Zenject;

namespace Player
{
    public class LevelNotesViewController
    {
        [Inject] private IPointerDetectionService _pointerDetectionService;
        [Inject] private IUIService _uIService;

        private KeyHintInfoView _keyHintInfoView;
        private Transform _currentNoteTransform;

        public void Initialize()
        {
            _pointerDetectionService.OnItemDetected += ItemDetected;
            _pointerDetectionService.OnItemMissed += ItemMissed;

            _keyHintInfoView = _uIService.GetScreen<PlayerHud, object, object>().KeyHintInfoView;
        }

        public void Cleanup()
        {
            _pointerDetectionService.OnItemDetected -= ItemDetected;
            _pointerDetectionService.OnItemMissed -= ItemMissed;
        }

        private void ItemDetected(Transform item)
        {
            if (item.GetComponent<ILevelNote>() == null) return;

            _keyHintInfoView.ShowHintText("[E] - Read note");
            _currentNoteTransform = item;
        }

        private void ItemMissed(Transform item)
        {
            if (_currentNoteTransform == null) return;
            if (item != _currentNoteTransform) return;

            _keyHintInfoView.HideHintText();
            _currentNoteTransform = null;
        }
    }
}


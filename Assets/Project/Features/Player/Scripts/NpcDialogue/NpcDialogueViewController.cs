using UI;
using UI.PlayerHud;
using UnityEngine;
using Zenject;

namespace Player
{
    public class NpcDialogueViewController
    {
        [Inject] private IPointerDetectionService _pointerDetectionService;
        [Inject] private IUIService _uIService;

        private KeyHintInfoView _keyHintInfoView;
        private Transform _currentNpcTransform;

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
            if (item.GetComponent<IDialoguePartner>() == null) return;

            _keyHintInfoView.ShowHintText("[E] - Talk");
            _currentNpcTransform = item;
        }

        private void ItemMissed(Transform item)
        {
            if (_currentNpcTransform == null) return;
            if (item != _currentNpcTransform) return;

            _keyHintInfoView.HideHintText();
            _currentNpcTransform = null;
        }
    }
}

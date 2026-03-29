using System;
using UI;
using UI.PlayerHud;
using UnityEngine;
using Zenject;

namespace Player
{
    public class InteractionViewController : IInitializable, IDisposable
    {
        [Inject] private IPointerDetectionService _pointerDetectionService;
        [Inject] private IUIService _uIService;

        private KeyHintInfoView _keyHintInfoView;
        private Transform _currentCollectableTransform;

        public void Initialize()
        {
            _pointerDetectionService.OnItemDetected += ItemDetected;
            _pointerDetectionService.OnItemMissed += ItemMissed;
            _keyHintInfoView = _uIService.GetScreen<PlayerHud, object, object>().KeyHintInfoView;
        }


        public void Dispose()
        {
            _pointerDetectionService.OnItemDetected -= ItemDetected;
            _pointerDetectionService.OnItemMissed -= ItemMissed;
        }

        private void ItemDetected(Transform item)
        {
            if (item.GetComponent<IInteractable>() != null)
            {
                _keyHintInfoView.ShowHintText("[E] - Interact");
                _currentCollectableTransform = item;
            }
        }

        private void ItemMissed(Transform item)
        {
            if (_currentCollectableTransform == null) return;

            if (item == _currentCollectableTransform)
            {
                _keyHintInfoView.HideHintText();
                _currentCollectableTransform = null;
            }
        }
    }
}
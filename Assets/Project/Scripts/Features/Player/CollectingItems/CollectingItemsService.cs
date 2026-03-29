using System;
using DG.Tweening;
using Input;
using UnityEngine;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public class CollectingItemsService : ICollectingItemsService
    {
        [Inject] private IPointerDetectionService _pointerDetectionService;
        [Inject] private IInventory _inventory;
        [Inject] private PlayerInput _playerInput;
        [Inject] private PlayerPresenter _playerPresenter;

        public void Initialize()
        {
            _playerInput.Player.Interact.started += TryCollectItem;
        }

        public void Cleanup()
        {
            _playerInput.Player.Interact.started -= TryCollectItem;
        }

        private void TryCollectItem(CallbackContext callbackContext)
        {
            if (_pointerDetectionService.ItemInView == null) return;

            if (_pointerDetectionService.ItemInView.TryGetComponent(out ICollectableItem collectableItem))
            {
                _inventory.AddItem(new InventoryItemData(collectableItem.ItemData.Id,
                    collectableItem.ItemData.Icon));
                CollectItem(collectableItem);
            }
        }

        private void CollectItem(ICollectableItem collectableItem)
        {
            collectableItem.OnCollect();
            DOTween.Kill(collectableItem.ItemTransform);
            collectableItem.ItemTransform.DOMove(_playerPresenter.CollectItemsPoint.position, 0.2f)
            .SetId(collectableItem.ItemTransform)
            .SetLink(collectableItem.ItemTransform.gameObject)
            .SetEase(Ease.Flash)
            .OnComplete(() => GameObject.Destroy(collectableItem.ItemTransform.gameObject));
        }
    }
}
using System;
using DG.Tweening;
using Input;
using UnityEngine;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public class CollectingItemsService : ICollectingItemsService, IInitializable, IDisposable
    {
        [Inject] private IPointerDetectionService pointerDetectionService;
        [Inject] private IInventory inventory;
        [Inject] private PlayerInput playerInput;
        [Inject] private PlayerPresenter playerPresenter;

        public void Initialize()
        {
            playerInput.Player.Interact.started += TryCollectItem;
        }

        public void Dispose()
        {
            playerInput.Player.Interact.started -= TryCollectItem;
        }

        private void TryCollectItem(CallbackContext callbackContext)
        {
            if (pointerDetectionService.ItemInView == null) return;

            if (pointerDetectionService.ItemInView.TryGetComponent(out ICollectableItem collectableItem))
            {
                inventory.AddItem(new InventoryItemData(collectableItem.ItemData.Id,
                    collectableItem.ItemData.Icon));
                CollectItem(collectableItem);
            }
        }

        private void CollectItem(ICollectableItem collectableItem)
        {
            collectableItem.OnCollect();
            DOTween.Kill(collectableItem.ItemTransform);
            collectableItem.ItemTransform.DOMove(playerPresenter.CollectItemsPoint.position, 0.2f)
            .SetId(collectableItem.ItemTransform)
            .SetLink(collectableItem.ItemTransform.gameObject)
            .SetEase(Ease.Flash)
            .OnComplete(() => GameObject.Destroy(collectableItem.ItemTransform.gameObject));
        }
    }
}
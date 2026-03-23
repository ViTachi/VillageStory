using System;
using UI;
using UI.PlayerHud;
using UnityEngine;
using Zenject;

namespace Player
{
    public class InteractionViewController : IInitializable, IDisposable
    {
        [Inject] private IPointerDetectionService pointerDetectionService;
        [Inject] private IUIService uIService;

        private KeyHintInfoView keyHintInfoView;
        private Transform currentCollectableTransform;

        public void Initialize()
        {
            pointerDetectionService.OnItemDetected += ItemDetected;
            pointerDetectionService.OnItemMissed += ItemMissed;
            keyHintInfoView = uIService.GetScreen<PlayerHud, object, object>().KeyHintInfoView;
        }


        public void Dispose()
        {
            pointerDetectionService.OnItemDetected -= ItemDetected;
            pointerDetectionService.OnItemMissed -= ItemMissed;
        }

        private void ItemDetected(Transform item)
        {
            if (item.GetComponent<IInteractable>() != null)
            {
                keyHintInfoView.ShowHintText("[E] - Interact");
                currentCollectableTransform = item;
            }
        }

        private void ItemMissed(Transform item)
        {
            if (currentCollectableTransform == null) return;

            if (item == currentCollectableTransform)
            {
                keyHintInfoView.HideHintText();
                currentCollectableTransform = null;
            }
        }
    }
}
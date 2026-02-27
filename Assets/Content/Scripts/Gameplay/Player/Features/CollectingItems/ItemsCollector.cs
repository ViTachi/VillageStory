using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class ItemsCollector : MonoBehaviour
    {
        [SerializeField] private Transform _collectPoint;

        [Inject] private IInventory _inventory;
        [Inject] private IItemDetector _itemDetector;

        public event Action<ICollectable> OnExitCollectable;
        public event Action<ICollectable> OnEnterCollectable;
        private ICollectable _currentCollectable = null;

        public void OnCollect()
        {
            if (_currentCollectable == null) return;

            _currentCollectable.OnCollect();
            _inventory.AddItem(_currentCollectable.ItemId);
            Transform collectableTransform = _currentCollectable.Transform;
            _currentCollectable = null;

            DOTween.Kill(collectableTransform);
            collectableTransform.DOMove(_collectPoint != null ?
            _collectPoint.position : transform.position, 0.2f)
            .SetId(collectableTransform)
            .SetEase(Ease.Flash)
            .OnComplete(() => Destroy(collectableTransform.gameObject));
        }

        private void OnEnable()
        {
            _itemDetector.OnItemDetected += OnPointerEnterItem;
            _itemDetector.OnItemMissed += OnPointerExitItem;
        }

        private void OnDisable()
        {
            _itemDetector.OnItemDetected -= OnPointerEnterItem;
            _itemDetector.OnItemMissed -= OnPointerExitItem;
        }

        private void OnPointerEnterItem(Transform item)
        {
            ICollectable collectable = item.GetComponent<ICollectable>();
            if (collectable != null)
            {
                _currentCollectable = collectable;
                OnEnterCollectable?.Invoke(collectable);
            }
        }

        private void OnPointerExitItem(Transform item)
        {
            ICollectable collectable = item.GetComponent<ICollectable>();
            if (collectable != null)
            {
                _currentCollectable = null;
                OnExitCollectable?.Invoke(collectable);
            }
        }
    }
}
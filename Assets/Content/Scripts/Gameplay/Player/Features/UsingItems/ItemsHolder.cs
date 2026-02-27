using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class ItemsHolder : MonoBehaviour
    {
        [SerializeField] private Transform _itemParent;
        [Inject] private IItemDetector _itemDetector;

        public event Action<IholdableItem> OnExitHoldable;
        public event Action<IholdableItem> OnEnterHoldable;
        private IholdableItem _holdableInView = null;
        private IholdableItem _holdableInHands = null;

        private IUseItemContextProvider _contextProvider;

        private void Awake()
        {
            _contextProvider = GetComponent<IUseItemContextProvider>();
        }
        public void OnGrab()
        {
            if (_holdableInView == null) return;

            _holdableInView.OnGrab();
            Transform holdableTransform = _holdableInView.Transform;

            DOTween.Kill(holdableTransform);
            holdableTransform.SetParent(_itemParent);
            holdableTransform.DOLocalMove(_holdableInView.HandsOffset, 0.2f)
            .SetId(holdableTransform)
            .SetEase(Ease.Flash);
            holdableTransform.DOLocalRotate(_holdableInView.HandsRotation, 0.2f, RotateMode.Fast)
            .SetId(holdableTransform)
            .SetEase(Ease.Flash);

            _holdableInHands = _holdableInView;
            _holdableInView = null;
        }

        public void OnUseItem()
        {
            if (_holdableInHands != null && _holdableInHands is IUsableItem usable)
                usable.Use(_contextProvider.GetContext());
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
            IholdableItem holdable = item.GetComponent<IholdableItem>();
            if (holdable != null)
            {
                _holdableInView = holdable;
                OnEnterHoldable?.Invoke(holdable);
            }
        }

        private void OnPointerExitItem(Transform item)
        {
            IholdableItem holdable = item.GetComponent<IholdableItem>();
            if (holdable != null)
            {
                _holdableInView = null;
                OnExitHoldable?.Invoke(holdable);
            }
        }
    }
}

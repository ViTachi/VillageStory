using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class CollectingInfoView : MonoBehaviour
    {
        [SerializeField] private RectTransform _view;
        [SerializeField] private TMPro.TMP_Text _nameLabel;

        [Inject] private ItemsCollector _collector;
        [Inject] private Camera _camera;

        private ICollectable _collectable = null;

        private void Awake()
        {
            _view.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            PointerExitCollectableItem(null);
            _collector.OnEnterCollectable += OnPointerEnterCollectableItem;
            _collector.OnExitCollectable += PointerExitCollectableItem;
        }

        private void OnDisable()
        {
            _collector.OnEnterCollectable -= OnPointerEnterCollectableItem;
            _collector.OnExitCollectable -= PointerExitCollectableItem;
        }

        private void OnPointerEnterCollectableItem(ICollectable collectable)
        {
            DOTween.Kill(_view);
            _nameLabel.text = collectable.ItemName;
            _view.gameObject.SetActive(true);
            _view.DOScale(1, 0.15f).SetId(_view);
            _collectable = collectable;
        }

        private void PointerExitCollectableItem(ICollectable collectable)
        {
            DOTween.Kill(_view);
            _view.DOScale(0, 0.1f).SetId(_view)
            .OnComplete(() => _view.gameObject.SetActive(false));
            _collectable = null;
        }

        private void LateUpdate()
        {
            if (_collectable != null && _collectable.Transform != null)
            {
                _view.position = _camera.WorldToScreenPoint(_collectable.InfoPosition);
            }
        }
    }
}
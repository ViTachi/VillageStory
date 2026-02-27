using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class InteractionInfoView : MonoBehaviour
    {
        [SerializeField] private RectTransform _view;
        [SerializeField] private TMPro.TMP_Text _interactionLabel;
        [SerializeField] private TMPro.TMP_Text _requirementLabel;

        [Inject] private Interactor _interactor;
        [Inject] private Camera _camera;

        private IInteractable _interactable = null;

        private void Awake()
        {
            _view.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            PointerExitInteractableItem(null);
            _interactor.OnEnterInteractable += OnPointerEnterInteractableItem;
            _interactor.OnExitInteractable += PointerExitInteractableItem;
        }

        private void OnDisable()
        {
            _interactor.OnEnterInteractable -= OnPointerEnterInteractableItem;
            _interactor.OnExitInteractable -= PointerExitInteractableItem;
        }

        private void OnPointerEnterInteractableItem(IInteractable interactable, string requirementInfo)
        {
            DOTween.Kill(_view);
            _requirementLabel.text = requirementInfo;
            _interactionLabel.gameObject.SetActive(string.IsNullOrEmpty(requirementInfo));
            _requirementLabel.gameObject.SetActive(!string.IsNullOrEmpty(requirementInfo));

            _view.gameObject.SetActive(true);
            _view.DOScale(1, 0.15f).SetId(_view);
            _interactable = interactable;
        }

        private void PointerExitInteractableItem(IInteractable interactable)
        {
            DOTween.Kill(_view);
            _view.DOScale(0, 0.1f).SetId(_view)
           .OnComplete(() => _view.gameObject.SetActive(false));
            _interactable = null;
        }

        private void LateUpdate()
        {
            if (_interactable != null)
            {
                _view.position = _camera.WorldToScreenPoint(_interactable.InfoPosition);
            }
        }
    }
}
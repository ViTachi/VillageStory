using System;
using UnityEngine;
using Zenject;

public class Interactor : MonoBehaviour
{
    [Inject] private IItemDetector _itemDetector;

    public event Action<IInteractable> OnExitInteractable;
    public event Action<IInteractable, string> OnEnterInteractable;

    private IInteractable _currentInteractable;
    private IInteractionContextProvider _contextProvider;

    private void Awake()
    {
        _contextProvider = GetComponent<IInteractionContextProvider>();
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

    public void OnInteract()
    {
        if (_currentInteractable == null) return;

        var context = _contextProvider.GetContext();

        if (_currentInteractable.CanInteract(context, out string failReason))
        {
            _currentInteractable.Interact(context);
        }
    }

    private void OnPointerEnterItem(Transform item)
    {
        IInteractable interactable = item.GetComponent<IInteractable>();
        if (interactable != null)
        {
            _currentInteractable = interactable;
            var context = _contextProvider.GetContext();
            string info = string.Empty;
            if (!interactable.CanInteract(context, out string failReason))
            {
                info = failReason;
            }
            OnEnterInteractable?.Invoke(interactable, info);
        }
    }

    private void OnPointerExitItem(Transform item)
    {
        IInteractable interactable = item.GetComponent<IInteractable>();
        if (interactable != null)
        {
            _currentInteractable = null;
            OnExitInteractable?.Invoke(interactable);
        }
    }
}

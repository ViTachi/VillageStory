using UnityEngine;
using UnityEngine.Events;

public class InteractableToggle : InteractableItem
{
    [SerializeField] private bool _on;
    [SerializeField] private UnityEvent _onEnable;
    [SerializeField] private UnityEvent _onDisable;

    private void Start()
    {
        Apply();
    }

    public override void Interact(IInteractionContext interactionContext)
    {
        base.Interact(interactionContext);
        _on = !_on;
        Apply();
    }

    private void Apply()
    {
        if (_on) _onEnable?.Invoke();
        else _onDisable?.Invoke();
    }
}

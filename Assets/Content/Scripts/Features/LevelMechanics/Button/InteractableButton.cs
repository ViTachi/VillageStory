using UnityEngine;
using UnityEngine.Events;

public class InteractableButton : InteractableItem
{
    [SerializeField] private UnityEvent _onPress;

    public override void Interact(IInteractionContext context)
    {
        _onPress?.Invoke();
    }
}

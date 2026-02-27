using UnityEngine;

namespace Gameplay
{
    public interface IInteractable
    {
        bool CanInteract(IInteractionContext context, out string failReason);
        void Interact(IInteractionContext context);
        Vector3 InfoPosition { get; }
    }
}
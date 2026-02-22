using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _infoPoint;
    [SerializeReference]
    private List<InteractionRequirement> _requirements = new();

    public Vector3 InfoPosition => _infoPoint != null ? _infoPoint.position : transform.position;

    public virtual bool CanInteract(IInteractionContext context, out string failReason)
    {
        foreach (var req in _requirements)
        {
            if (!req.Check(context, out failReason))
                return false;
        }

        failReason = null;
        return true;
    }

    public virtual void Interact(IInteractionContext context)
    {
        
    }
}

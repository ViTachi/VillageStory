using UnityEngine;

[System.Serializable]
public class RequireItem : InteractionRequirement
{
    [SerializeField] private GameItemData _requiredItem;

    public override bool Check(IInteractionContext context, out string failReason)
    {
        if (context.Inventory.HasItem(_requiredItem.Id))
        {
            failReason = null;
            return true;
        }

        failReason = $"Required: {_requiredItem.ItemName}";
        return false;
    }
}

[System.Serializable]
public abstract class InteractionRequirement
{
    public abstract bool Check(IInteractionContext context, out string failReason);
}
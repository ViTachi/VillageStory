namespace Player
{
    /// <summary>
    /// Marker + entry point for NPCs the player can talk to (raycast / pointer detection).
    /// </summary>
    public interface IDialoguePartner
    {
        void BeginDialogue();

        void EndDialogue();
    }
}

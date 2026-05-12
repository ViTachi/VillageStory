namespace Player
{
    /// <summary>
    /// Marker + entry point for NPCs the player can talk to (raycast / pointer detection).
    /// </summary>
    public interface IDialoguePartner
    {
        DialogueData GetDialogueData();

        void BeginDialogue();

        void EndDialogue();
    }
}

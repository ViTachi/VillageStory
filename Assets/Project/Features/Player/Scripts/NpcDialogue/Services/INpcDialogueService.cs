using System;

namespace Player
{
    public interface INpcDialogueService
    {
        event Action OnShowDialogue;
        event Action OnHideDialogue;

        void Initialize();
        void Cleanup();
    }
}

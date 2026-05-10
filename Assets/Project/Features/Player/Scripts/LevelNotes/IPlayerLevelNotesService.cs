using System;

namespace Player
{
    public interface ILevelNotesService
    {
        event Action OnShowNote;
        event Action OnHideNote;
        void Initialize();
        void Cleanup();
    }
}


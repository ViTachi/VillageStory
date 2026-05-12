namespace Player
{
    public class PlayerStateMachine : StateMachine
    {
        public override void Initialize()
        {
            Add<InitializePlayerState>();
            Add<MoveState>();
            Add<ReadNoteState>();
            Add<NpcDialogueState>();
            Add<CleanupPlayerState>();

            EnterState<InitializePlayerState>();
        }
    }
}
namespace Player
{
    public class PlayerStateMachine : StateMachine
    {
        public override void Initialize()
        {
            Add<InitializePlayerState>();
            Add<MoveState>();
            Add<ReadNoteState>();
            Add<CleanupPlayerState>();

            EnterState<InitializePlayerState>();
        }
    }
}
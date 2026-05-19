namespace GameFlow
{
    public class GameFlowStateMachine : StateMachine
    {
        private bool _isInitialized;

        public override void Initialize()
        {
            if (_isInitialized)
                return;

            _isInitialized = true;

            Add<BootstrapState>();
            Add<MainMenuState>();
            Add<LevelLoadingState>();
            Add<GameplayState>();

            EnterState<BootstrapState>();
        }
    }
}

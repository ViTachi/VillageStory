using System.Threading;
using Cysharp.Threading.Tasks;
using UI;
using UI.Screens;
using Zenject;

namespace GameFlow
{
    /// <summary>Main menu; start level from UI.</summary>
    public class MainMenuState : State
    {
        [Inject] private IUIService _uiService;

        private bool _startRequested;

        public override async UniTask Enter(CancellationToken token)
        {
            _startRequested = false;

            MainMenuScreen screen = await _uiService.ShowScreen<MainMenuScreen, MainMenuScreen.Parameters, MainMenuScreen.Result>(
                UILayer.Main,
                new MainMenuScreen.Parameters(),
                false,
                token,
                null);

            screen.StartGameRequested += OnStartGameRequested;

            await UniTask.WaitUntil(() => _startRequested, cancellationToken: token);

            screen.StartGameRequested -= OnStartGameRequested;

            await _uiService.HideScreen<MainMenuScreen>(false, token);

            _stateMachine.EnterState<LevelLoadingState>();
        }

        private void OnStartGameRequested()
        {
            _startRequested = true;
        }
    }
}

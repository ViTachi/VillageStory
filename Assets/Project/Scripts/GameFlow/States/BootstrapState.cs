using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UI;
using UI.Screens;
using Zenject;

namespace GameFlow
{
    /// <summary>Initial game entry: settings and bootstrap UI.</summary>
    public class BootstrapState : State
    {
        [Inject] private GameConfig _gameConfig;
        [Inject] private IUIService _uiService;
        [Inject] private ISceneLoadService _sceneLoadService;

        public override async UniTask Enter(CancellationToken token)
        {
            await _uiService.ShowScreen<BootstrapScreen, BootstrapScreen.Parameters, BootstrapScreen.Result>(
                UILayer.Main,
                new BootstrapScreen.Parameters { i = 0 },
                false,
                token,
                null);

            await UniTask.Delay(500, cancellationToken: token);

            await _uiService.HideScreen<BootstrapScreen>(false, token);

            await _sceneLoadService.LoadSceneAsync(SceneNames.MainMenu, LoadSceneMode.Additive, token);
            _sceneLoadService.SetActiveScene(SceneNames.MainMenu);
            await _sceneLoadService.UnloadSceneAsync(SceneNames.Entry, token);

            _stateMachine.EnterState<MainMenuState>();
        }
    }
}

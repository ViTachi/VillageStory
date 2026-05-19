using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Zenject;

namespace GameFlow
{
    /// <summary>Loads the gameplay level scene.</summary>
    public class LevelLoadingState : State
    {
        [Inject] private ISceneLoadService _sceneLoadService;

        public override async UniTask Enter(CancellationToken token)
        {
            await _sceneLoadService.LoadSceneAsync(SceneNames.Level, LoadSceneMode.Additive, token);
            _sceneLoadService.SetActiveScene(SceneNames.Level);
            await _sceneLoadService.UnloadSceneAsync(SceneNames.MainMenu, token);

            _stateMachine.EnterState<GameplayState>();
        }
    }
}

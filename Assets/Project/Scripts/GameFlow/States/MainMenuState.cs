using System.Threading;
using Cysharp.Threading.Tasks;
using UI;
using UI.Screens;
using UnityEngine;
using Zenject;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameFlow
{
    /// <summary>Main menu; start level, settings, or exit.</summary>
    public class MainMenuState : State
    {
        private enum MainMenuAction
        {
            StartGame,
            Settings,
            Exit
        }

        [Inject] private IUIService _uiService;
        [Inject] private ISettingsService _settingsService;

        public override async UniTask Enter(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                MainMenuAction action = await WaitForMainMenuActionAsync(token);

                switch (action)
                {
                    case MainMenuAction.StartGame:
                        _stateMachine.EnterState<LevelLoadingState>();
                        return;
                    case MainMenuAction.Settings:
                        await OpenSettingsAsync(token);
                        break;
                    case MainMenuAction.Exit:
                        QuitGame();
                        return;
                }
            }
        }

        private async UniTask<MainMenuAction> WaitForMainMenuActionAsync(CancellationToken token)
        {
            MainMenuScreen screen = await _uiService.ShowScreen<MainMenuScreen, MainMenuScreen.Parameters, MainMenuScreen.Result>(
                UILayer.Main,
                new MainMenuScreen.Parameters(),
                false,
                token,
                null);

            var actionSource = new UniTaskCompletionSource<MainMenuAction>();

            void OnStartGame() => actionSource.TrySetResult(MainMenuAction.StartGame);
            void OnSettings() => actionSource.TrySetResult(MainMenuAction.Settings);
            void OnExit() => actionSource.TrySetResult(MainMenuAction.Exit);

            screen.StartGameRequested += OnStartGame;
            screen.SettingsRequested += OnSettings;
            screen.ExitRequested += OnExit;

            MainMenuAction action = await actionSource.Task.AttachExternalCancellation(token);

            screen.StartGameRequested -= OnStartGame;
            screen.SettingsRequested -= OnSettings;
            screen.ExitRequested -= OnExit;

            await _uiService.HideScreen<MainMenuScreen>(false, token);
            return action;
        }

        private async UniTask OpenSettingsAsync(CancellationToken token)
        {
            SettingsScreen screen = await _uiService.ShowScreen<SettingsScreen, SettingsScreen.Parameters, object>(
                UILayer.Popup,
                new SettingsScreen.Parameters { SettingsService = _settingsService },
                false,
                token,
                null);

            var closedSource = new UniTaskCompletionSource();
            screen.Closed += OnSettingsClosed;

            await closedSource.Task.AttachExternalCancellation(token);

            screen.Closed -= OnSettingsClosed;
            await _uiService.HideScreen<SettingsScreen>(false, token);

            void OnSettingsClosed() => closedSource.TrySetResult();
        }

        private static void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}

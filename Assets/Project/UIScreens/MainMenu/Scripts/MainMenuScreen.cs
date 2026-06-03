using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

namespace UI.Screens
{
    public class MainMenuScreen : Screen<MainMenuScreen.Parameters, MainMenuScreen.Result>
    {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitButton;

        public event Action StartGameRequested;
        public event Action SettingsRequested;
        public event Action ExitRequested;

        public class Parameters
        {
        }

        public class Result
        {
            public bool success = false;
        }

        public override async UniTask Show(Parameters parameters, bool immediately,
            CancellationToken ct, Action<Result> callback = null)
        {
            await base.Show(parameters, immediately, ct, callback);

            _startGameButton.onClick.AddListener(OnStartGameClicked);
            _settingsButton.onClick.AddListener(OnSettingsClicked);
            _exitButton.onClick.AddListener(OnExitClicked);

            SetupMenuNavigation();
            FocusFirstButtonAsync().Forget();
        }

        public override UniTask Hide(bool immediately, CancellationToken ct)
        {
            _startGameButton.onClick.RemoveListener(OnStartGameClicked);
            _settingsButton.onClick.RemoveListener(OnSettingsClicked);
            _exitButton.onClick.RemoveListener(OnExitClicked);
            EventSystem.current?.SetSelectedGameObject(null);
            canvasGroup.interactable = false;
            return base.Hide(immediately, ct);
        }

        private void SetupMenuNavigation()
        {
            Button[] buttons = { _startGameButton, _settingsButton, _exitButton };

            for (int i = 0; i < buttons.Length; i++)
            {
                Button button = buttons[i];
                if (button == null)
                    continue;

                Navigation navigation = new Navigation { mode = Navigation.Mode.Explicit };
                navigation.selectOnUp = i > 0 ? buttons[i - 1] : null;
                navigation.selectOnDown = i < buttons.Length - 1 ? buttons[i + 1] : null;
                navigation.selectOnLeft = button;
                navigation.selectOnRight = button;
                button.navigation = navigation;

                if (button.GetComponent<MenuButtonHoverSelect>() == null)
                    button.gameObject.AddComponent<MenuButtonHoverSelect>();
            }
        }

        private async UniTaskVoid FocusFirstButtonAsync()
        {
            if (_startGameButton == null)
                return;

            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);

            EventSystem eventSystem = EventSystem.current;
            if (eventSystem == null)
                return;

            eventSystem.SetSelectedGameObject(_startGameButton.gameObject);
        }

        private void OnStartGameClicked()
        {
            result = new Result { success = true };
            StartGameRequested?.Invoke();
        }

        private void OnSettingsClicked()
        {
            SettingsRequested?.Invoke();
        }

        private void OnExitClicked()
        {
            ExitRequested?.Invoke();
        }
    }

    internal class MenuButtonHoverSelect : MonoBehaviour, IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            EventSystem.current?.SetSelectedGameObject(gameObject);
        }
    }
}

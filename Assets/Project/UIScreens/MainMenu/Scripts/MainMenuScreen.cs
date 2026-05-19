using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine.UI;
using UnityEngine;

namespace UI.Screens
{
    public class MainMenuScreen : Screen<MainMenuScreen.Parameters, MainMenuScreen.Result>
    {
        [SerializeField] private Button _startGameButton;

        public event Action StartGameRequested;

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
        }

        public override UniTask Hide(bool immediately, CancellationToken ct)
        {
            _startGameButton.onClick.RemoveListener(OnStartGameClicked);
            canvasGroup.interactable = false;
            return base.Hide(immediately, ct);
        }

        private void OnStartGameClicked()
        {
            result = new Result { success = true };
            StartGameRequested?.Invoke();
        }
    }
}

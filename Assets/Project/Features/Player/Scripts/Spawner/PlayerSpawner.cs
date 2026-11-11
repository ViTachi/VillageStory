using System.Threading;
using Cysharp.Threading.Tasks;
using Input;
using UI;
using UI.PlayerHud;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerSpawner
    {
        [Inject] private DiContainer _diContainer;
        [Inject] private GameConfig _gameConfig;
        [Inject] private PlayerInput _playerInput;
        [Inject] private IUIService _uIService;

        public async UniTask SpawnPlayer(Transform root, Vector3 position)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            await ShowPlayerHud(cts.Token);

            GameObject player = _diContainer.InstantiatePrefab(
                _gameConfig.PlayerPrefab,
                position, Quaternion.identity, root);

            PlayerCursor.SetLockedForGameplay();
            _playerInput.Enable();
        }

        private async UniTask ShowPlayerHud(CancellationToken token)
        {

            await _uIService.ShowScreen<PlayerHud, object, object>(
                UILayer.Main, null, true, token, null);
        }
    }
}
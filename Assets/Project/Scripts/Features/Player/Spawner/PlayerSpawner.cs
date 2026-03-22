using System.Threading;
using System.Threading.Tasks;
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
        [Inject] private DiContainer diContainer;
        [Inject] private GameConfig gameConfig;
        [Inject] private PlayerInput playerInput;
        [Inject] private IUIService uIService;

        public async UniTask SpawnPlayer(Transform root, Vector3 position)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            await ShowPlayerHud(cts.Token);

            GameObject player = diContainer.InstantiatePrefab(gameConfig.PlayerPrefab);
            player.transform.SetParent(root);
            player.transform.position = position;

            SetCursorLocked(true);
            playerInput.Enable();
        }

        private async UniTask ShowPlayerHud(CancellationToken token)
        {

            await uIService.ShowScreen<PlayerHud, object, object>(
                UILayer.Main, null, false, token, null);
        }

        private void SetCursorLocked(bool locked)
        {
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = locked;
        }
    }
}
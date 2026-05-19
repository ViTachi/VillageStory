using System.Threading;
using Cysharp.Threading.Tasks;
using Player;
using UnityEngine;
using Zenject;

namespace GameFlow
{
    /// <summary>Active gameplay: spawn player in the loaded level.</summary>
    public class GameplayState : State
    {
        [Inject] private ISceneLoadService _sceneLoadService;
        [Inject] private PlayerSpawner _playerSpawner;

        public override async UniTask Enter(CancellationToken token)
        {
            PlayerSpawnPoint spawnPoint = Object.FindAnyObjectByType<PlayerSpawnPoint>();
            if (spawnPoint == null)
            {
                Debug.LogError("GameplayState: PlayerSpawnPoint not found in level scene.");
                return;
            }

            await _playerSpawner.SpawnPlayer(spawnPoint.transform, spawnPoint.transform.position);
        }
    }
}

using Cysharp.Threading.Tasks;
using Player;
using UnityEngine;
using Zenject;

public class Entry : MonoBehaviour
{
    [SerializeField] private Transform _playerPosition;
    [Inject] private PlayerSpawner _playerSpawner;
    private void Start()
    {
        _playerSpawner.SpawnPlayer(transform, _playerPosition.position).Forget();
    }
}
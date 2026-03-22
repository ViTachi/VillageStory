using Cysharp.Threading.Tasks;
using Player;
using UnityEngine;
using Zenject;

public class Entry : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;
    [Inject] private PlayerSpawner playerSpawner;
    private void Start()
    {
        playerSpawner.SpawnPlayer(transform, playerPosition.position).Forget();
    }
}
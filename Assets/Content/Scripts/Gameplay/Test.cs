using Gameplay;
using UnityEngine;
using Zenject;

public class Test : MonoBehaviour
{
    [SerializeField] private BotUnit _unit;
    [Inject] private DiContainer _diContainer;

    private void Awake()
    {
        _diContainer.InstantiatePrefabForComponent<BotUnit>(_unit);
    }
}

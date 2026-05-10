using Input;
using Player;
using UI;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private CinemachineBrain _cinemachineBrainPrefab;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private UIService _uIServicePrefab;

    public override void InstallBindings()
    {
        Container.BindInterfacesTo<FileService>().AsSingle();
        Container.Bind<PlayerInput>().AsSingle();
        Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle();
        Container.Bind<PlayerSpawner>().AsSingle();
        Container.Bind<CinemachineBrain>().FromComponentInNewPrefab(_cinemachineBrainPrefab).AsSingle();
        Container.Bind<EventSystem>().FromComponentInNewPrefab(_eventSystem).AsSingle();
        Container.BindInterfacesTo<UIService>().FromComponentInNewPrefab(_uIServicePrefab).AsSingle();

        SetupGame();
    }

    private void SetupGame()
    {
        Application.targetFrameRate = _gameConfig.TargetFPS;
    }
}

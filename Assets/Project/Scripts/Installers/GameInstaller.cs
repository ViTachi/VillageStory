using GameFlow;
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
        Container.BindInterfacesTo<SaveLoadService>().AsSingle().NonLazy();
        Container.BindInterfacesTo<SettingsService>().AsSingle().NonLazy();
        Container.Bind<PlayerInput>().AsSingle();
        Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle();
        Container.Bind<PlayerSpawner>().AsSingle();
        Container.Bind<CinemachineBrain>().FromComponentInNewPrefab(_cinemachineBrainPrefab)
            .AsSingle().NonLazy();
        Container.Bind<EventSystem>().FromComponentInNewPrefab(_eventSystem)
            .AsSingle().NonLazy();
        Container.BindInterfacesTo<UIService>().FromComponentInNewPrefab(_uIServicePrefab)
            .AsSingle().NonLazy();
        Container.BindInterfacesTo<SceneLoadService>().AsSingle();
        Container.Bind<GameFlowStateMachine>().AsSingle();

        SetupGame();
    }

    private void SetupGame()
    {
        Application.targetFrameRate = _gameConfig.TargetFPS;
    }
}

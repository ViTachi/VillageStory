using Gameplay;
using Input;
using Player;
using UI;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameConfig gameConfig;
    [SerializeField] private CinemachineBrain cinemachineBrainPrefab;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private UIService uIServicePrefab;

    public override void InstallBindings()
    {
        Container.BindInterfacesTo<FileService>().AsSingle();
        Container.Bind<PlayerInput>().AsSingle();
        Container.Bind<GameConfig>().FromInstance(gameConfig).AsSingle();
        Container.Bind<PlayerSpawner>().AsSingle();
        Container.Bind<CinemachineBrain>().FromComponentInNewPrefab(cinemachineBrainPrefab).AsSingle();
        Container.Bind<EventSystem>().FromComponentInNewPrefab(eventSystem).AsSingle();
        Container.BindInterfacesTo<UIService>().FromComponentInNewPrefab(uIServicePrefab).AsSingle();

        SetupGame();
    }

    private void SetupGame()
    {
        Application.targetFrameRate = 60;
    }
}

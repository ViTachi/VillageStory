using Services;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private UIService _uIServicePrefab;
    public override void InstallBindings()
    {
        Container.Bind<EventSystem>().FromComponentsInNewPrefab(_eventSystem)
            .AsSingle()
            .NonLazy();
        Container.Bind<GameSettingsData>().FromInstance(_gameSettings.Settings)
            .AsSingle()
            .NonLazy();
        Container.BindInterfacesTo<UIService>().FromComponentInNewPrefab(_uIServicePrefab)
            .AsSingle()
            .NonLazy();
    }
}

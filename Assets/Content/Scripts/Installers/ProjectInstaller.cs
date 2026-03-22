using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Core.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private UIService _uIServicePrefab;

        public override void InstallBindings()
        {
            Container.Bind<EventSystem>().FromComponentsInNewPrefab(_eventSystem)
                .AsSingle()
                .NonLazy();
            Container.BindInterfacesTo<UIService>().FromComponentInNewPrefab(_uIServicePrefab)
                .AsSingle()
                .NonLazy();
        }
    }
}
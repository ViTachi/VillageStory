using Gameplay;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class LevelSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<PathFinder>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.BindInterfacesTo<NoiseHandler>().AsSingle().NonLazy();
            Container.Bind<Location>().FromComponentInHierarchy().AsSingle().NonLazy();
        }
    }

    public class PlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PlayerMovementService>().FromNew().AsSingle();
        }
    }

    public class PlayerView : MonoBehaviour
    {
        [field: SerializeField] public Transform Root { get; private set; }
    }

    public class PlayerMovementService
    {
        [Inject] private PlayerView _playerView;
    }
}
using Gameplay;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class LevelSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<PathFindService>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.BindInterfacesTo<NoiseHandler>().AsSingle().NonLazy();
            Container.Bind<Location>().FromComponentInHierarchy().AsSingle().NonLazy();
        }
    }
}
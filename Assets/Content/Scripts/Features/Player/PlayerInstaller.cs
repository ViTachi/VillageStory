using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Camera>().FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();
        Container.Bind<IItemDetector>().To<RaycastItemDetector>().FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();
        Container.Bind<Interactor>().FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();
        Container.Bind<ItemsCollector>().FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();
        Container.Bind<ItemsHolder>().FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();
        Container.Bind<IInventory>().To<PlayerInventory>().FromNew()
            .AsSingle()
            .NonLazy();
    }
}

using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Gameplay.Intsallers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private FPCameraController _fPCameraController;
        [SerializeField] private Camera _camera;
        [SerializeField] private RaycastItemDetector _raycastItemDetector;
        [SerializeField] private GameObject _itemsDetector;
        [SerializeField] private Interactor _interactor;
        [SerializeField] private ItemsCollector _itemsCollector;
        [SerializeField] private ItemsHolder _itemsHolder;
        [SerializeField] private CinemachineCamera _cinemachineCamera;
        public override void InstallBindings()
        {
            Container.Bind<IPlayerController>().FromInstance(_playerController)
                .AsSingle()
                .NonLazy();
            Container.Bind<IFPCameraController>().FromInstance(_fPCameraController)
                .AsSingle()
                .NonLazy();
            Container.Bind<Camera>().FromInstance(_camera)
                .AsSingle()
                .NonLazy();
            Container.Bind<IItemDetector>().To<RaycastItemDetector>().FromInstance(_raycastItemDetector)
                .AsSingle()
                .NonLazy();
            Container.Bind<Interactor>().FromInstance(_interactor)
                .AsSingle()
                .NonLazy();
            Container.Bind<ItemsCollector>().FromInstance(_itemsCollector)
                .AsSingle()
                .NonLazy();
            Container.Bind<ItemsHolder>().FromInstance(_itemsHolder)
                .AsSingle()
                .NonLazy();
            Container.Bind<IInventory>().To<UnitInventory>().FromNew()
                .AsSingle()
                .NonLazy();
            Container.Bind<CinemachineCamera>().FromInstance(_cinemachineCamera)
                .AsSingle()
                .NonLazy();
        }
    }
}

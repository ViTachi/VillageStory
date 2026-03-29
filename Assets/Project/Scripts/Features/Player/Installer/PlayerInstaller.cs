using Player;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private PlayerMovementConfig _playerMovementConfig;
        [SerializeField] private PlayerDetectionConfig _playerDetectionConfig;
        [SerializeField] private PlayerPresenter _playerPresenter;
        [SerializeField] private CinemachineCamera _cinemachineCameraPrefab;

        public override void InstallBindings()
        {
            BindConfigs();

            Container.BindInterfacesTo<PlayerMovementService>().AsSingle();
            Container.BindInterfacesTo<PlayerController>().FromInstance(_playerController).AsSingle();
            Container.Bind<PlayerPresenter>().FromInstance(_playerPresenter).AsSingle();
            Container.Bind<CinemachineCamera>().FromComponentInNewPrefab(_cinemachineCameraPrefab).AsSingle();
            Container.BindInterfacesTo<RaycastPointerDetectionService>().AsSingle();
            Container.BindInterfacesTo<Inventory>().AsSingle();

            Container.BindInterfacesTo<CollectingItemsService>().AsSingle();
            Container.BindInterfacesTo<CollectingItemsViewController>().AsSingle();

            Container.BindInterfacesTo<PlayerInteractService>().AsSingle();
            Container.BindInterfacesTo<InteractionViewController>().AsSingle();

            Container.BindInterfacesTo<LevelNotesService>().AsSingle();
            Container.BindInterfacesTo<LevelNotesViewController>().AsSingle();

            Container.Bind<PlayerStateMachine>().AsSingle().NonLazy();
        }

        private void BindConfigs()
        {
            Container.Bind<PlayerMovementConfig>().FromInstance(_playerMovementConfig).AsSingle();
            Container.Bind<PlayerDetectionConfig>().FromInstance(_playerDetectionConfig).AsSingle();
        }
    }
}
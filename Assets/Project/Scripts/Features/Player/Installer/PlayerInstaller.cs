using Gameplay;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerMovementConfig playerMovementConfig;
        [SerializeField] private PlayerDetectionConfig playerDetectionConfig;
        [SerializeField] private PlayerPresenter playerPresenter;
        [SerializeField] private CinemachineCamera cinemachineCameraPrefab;

        public override void InstallBindings()
        {
            BindConfigs();

            Container.BindInterfacesTo<PlayerMovementService>().AsSingle();
            Container.BindInterfacesTo<PlayerController>().FromInstance(playerController).AsSingle();
            Container.Bind<PlayerPresenter>().FromInstance(playerPresenter).AsSingle();
            Container.Bind<CinemachineCamera>().FromComponentInNewPrefab(cinemachineCameraPrefab).AsSingle();
            Container.BindInterfacesTo<RaycastPointerDetectionService>().AsSingle();
            Container.BindInterfacesTo<Inventory>().AsSingle();

            Container.BindInterfacesTo<CollectingItemsService>().AsSingle();
            Container.BindInterfacesTo<CollectingItemsViewController>().AsSingle();

            Container.BindInterfacesTo<PlayerInteractService>().AsSingle();
            Container.BindInterfacesTo<InteractionViewController>().AsSingle();
        }

        private void BindConfigs()
        {
            Container.Bind<PlayerMovementConfig>().FromInstance(playerMovementConfig).AsSingle();
            Container.Bind<PlayerDetectionConfig>().FromInstance(playerDetectionConfig).AsSingle();
        }
    }
}
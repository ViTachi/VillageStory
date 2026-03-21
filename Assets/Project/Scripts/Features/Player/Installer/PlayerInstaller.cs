using Gameplay;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerController playerController;
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<PlayerMovementService>().AsSingle();
            Container.BindInterfacesTo<PlayerController>().FromInstance(playerController).AsSingle();
        }
    }
}
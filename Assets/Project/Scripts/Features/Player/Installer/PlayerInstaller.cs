using Zenject;

namespace Player
{
    public class PlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<PlayerMovementService>().AsSingle();
        }
    }
}
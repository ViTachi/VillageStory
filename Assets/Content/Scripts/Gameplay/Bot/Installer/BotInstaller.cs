using Gameplay.AI;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Bot
{
    public class BotInstaller : MonoInstaller
    {
        [SerializeField] private UnitActionsLauncher _unitActionsLauncher;
        [SerializeField] private UnitSenses _unitSenses;
        public override void InstallBindings()
        {
            Container.Bind<UnitSenses>().FromInstance(_unitSenses).AsSingle().NonLazy();
            Container.Bind<Unit>().FromComponentsOn(gameObject).AsSingle().NonLazy();
            Container.Bind<UnitActionsLauncher>().FromInstance(_unitActionsLauncher).AsSingle().NonLazy();
        }
    }
}
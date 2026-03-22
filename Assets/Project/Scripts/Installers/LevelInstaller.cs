using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField] private LevelConfig levelConfig;
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<PathFindService>().AsSingle();
        Container.Bind<LevelConfig>().FromInstance(levelConfig).AsSingle();
    }
}

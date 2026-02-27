using Unity.Cinemachine;

namespace Gameplay
{
    public interface IUseItemContext
    {
        CinemachineCamera CinemachineCamera { get; }
    }
}
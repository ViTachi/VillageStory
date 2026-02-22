using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class PlayerUseItemContextProvider : MonoBehaviour, IUseItemContextProvider
{
    [SerializeField] private CinemachineCamera _cinemachineCamera;
    private PlayerUseItemContext _context;

    [Inject]
    private void Construct()
    {
        _context = new PlayerUseItemContext(_cinemachineCamera);
    }
    public IUseItemContext GetContext() => _context;
}

public class PlayerUseItemContext : IUseItemContext
{
    public CinemachineCamera CinemachineCamera { get; }

    public PlayerUseItemContext(CinemachineCamera camera)
    {
        CinemachineCamera = camera;
    }
}

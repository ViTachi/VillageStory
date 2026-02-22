using UnityEngine;
using Zenject;

public class PlayerInteractionContextProvider : MonoBehaviour, IInteractionContextProvider
{
    [Inject] private IInventory _inventory;

    private PlayerInteractionContext _context;

    [Inject]
    private void Construct()
    {
        _context = new PlayerInteractionContext(_inventory);
    }

    public IInteractionContext GetContext() => _context;
}

public class PlayerInteractionContext : IInteractionContext
{
    public IInventory Inventory { get; }

    public PlayerInteractionContext(IInventory inventory)
    {
        Inventory = inventory;
    }
}
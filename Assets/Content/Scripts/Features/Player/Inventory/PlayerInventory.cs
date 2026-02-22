using System;
using System.Collections.Generic;

public class PlayerInventory : IInventory
{
    private readonly List<InventoryItemData> _items = new();

    public IReadOnlyList<InventoryItemData> GetAllItems() => _items;

    public void AddItem(string id)
    {
        UnityEngine.Debug.Log($"Item {id} added to inventory");
        _items.Add(new InventoryItemData(id));
    }

    public void RemoveItem(Guid guid) => _items.RemoveAll(x => x.Guid == guid);

    public bool HasItem(string id) => _items.Exists(x => x.Id == id);
}

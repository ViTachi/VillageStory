using System;
using System.Collections.Generic;

public interface IInventory
{
    IReadOnlyList<InventoryItemData> GetAllItems();
    void AddItem(string id);
    void RemoveItem(Guid guid);
    bool HasItem(string id);
}

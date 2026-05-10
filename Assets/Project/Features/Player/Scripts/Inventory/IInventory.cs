using System;

namespace Player
{
    public interface IInventory
    {
        event Action OnChange;

        void AddItem(InventoryItemData itemData);
        bool HasItem(string id);
        void RemoveItem(string id);
    }
}
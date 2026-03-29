using System;
using System.Collections.Generic;
using System.Linq;

namespace Player
{
    public class Inventory : IInventory
    {
        public event Action OnChange;

        private List<InventoryItemData> _items = new List<InventoryItemData>();

        public bool HasItem(string id) =>
            _items.Exists(x => x.Id.Equals(id));

        public void AddItem(InventoryItemData itemData)
        {
            _items.Add(itemData);
            OnChange?.Invoke();
        }

        public void RemoveItem(string id)
        {
            InventoryItemData item = _items.FirstOrDefault(x => x.Id.Equals(id));
            _items.Remove(item);
            OnChange?.Invoke();
        }
    }
}
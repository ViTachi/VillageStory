using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player
{
    public class Inventory : IInventory
    {
        public event Action OnChange;

        private List<InventoryItemData> items = new List<InventoryItemData>();

        public bool HasItem(string id) =>
            items.Exists(x => x.Id.Equals(id));

        public void AddItem(InventoryItemData itemData)
        {
            items.Add(itemData);
            OnChange?.Invoke();
        }

        public void RemoveItem(string id)
        {
            InventoryItemData item = items.FirstOrDefault(x => x.Id.Equals(id));
            items.Remove(item);
            OnChange?.Invoke();
        }
    }
}
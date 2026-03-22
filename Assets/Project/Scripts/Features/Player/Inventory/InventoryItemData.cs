using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class InventoryItemData
    {
        public readonly string Id;
        public readonly Sprite Icon;

        public InventoryItemData(string id, Sprite icon)
        {
            Id = id;
            Icon = icon;
        }
    }
}
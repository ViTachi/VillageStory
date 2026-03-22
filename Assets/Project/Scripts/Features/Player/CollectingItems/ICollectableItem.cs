using UnityEngine;

namespace Player
{
    public interface ICollectableItem
    {
        CollectableItemData ItemData { get; }
        Transform ItemTransform { get; }
        void OnCollect();
    }
}
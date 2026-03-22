using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class CollectableItemData
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
    }
}
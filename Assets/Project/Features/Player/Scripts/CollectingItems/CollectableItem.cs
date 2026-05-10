using UnityEngine;

namespace Player
{
    public class CollectableItem : MonoBehaviour, ICollectableItem
    {
        [SerializeField] private CollectableItemData _itemData;
        public CollectableItemData ItemData => _itemData;
        public Transform ItemTransform => transform;

        public void OnCollect()
        {
            foreach (Collider collider in GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }
        }
    }
}
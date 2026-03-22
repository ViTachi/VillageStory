using UnityEngine;

namespace Player
{
    public class CollectableItem : MonoBehaviour, ICollectableItem
    {
        [SerializeField] private CollectableItemData itemData;
        public CollectableItemData ItemData => itemData;
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
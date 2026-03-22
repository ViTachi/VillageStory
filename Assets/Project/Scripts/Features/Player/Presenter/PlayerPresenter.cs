using UnityEngine;

namespace Player
{
    public class PlayerPresenter : MonoBehaviour
    {
        [field: SerializeField] public Transform CameraAnchor { get; private set; }
        [field: SerializeField] public Transform CollectItemsPoint { get; private set; }
    }
}
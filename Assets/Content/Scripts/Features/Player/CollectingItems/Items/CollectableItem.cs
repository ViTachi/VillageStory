using UnityEngine;

public class CollectableItem : MonoBehaviour, ICollectable
{
    [SerializeField] private GameItemData _itemData;
    [SerializeField] private Transform _infoPoint;
    [SerializeField] private Rigidbody _rigidBody;
    public string ItemId => _itemData.Id;

    public string ItemName => _itemData.ItemName;

    public Vector3 InfoPosition => _infoPoint ? _infoPoint.position : transform.position;

    public Transform Transform => transform;

    public void OnCollect()
    {
        foreach(var collider in GetComponentsInChildren<Collider>())
            collider.enabled = false;
        Destroy(_rigidBody);
    }
}

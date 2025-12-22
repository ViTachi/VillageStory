using UnityEngine;

public interface ICollectable
{
    string ItemId { get; }
    string ItemName { get; }
    Vector3 InfoPosition { get; }
    Transform Transform { get; }
    void OnCollect();
}

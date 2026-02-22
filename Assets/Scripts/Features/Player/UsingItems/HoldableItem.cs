using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HoldableItem : MonoBehaviour, IholdableItem
{
    [SerializeField] private Vector3 _handsOffset;
    [SerializeField] private Vector3 _handsRotation;
    [SerializeField] private Rigidbody _rigidbody;

    private List<Collider> _colliders;
    public Vector3 HandsOffset => _handsOffset;

    public Transform Transform => transform;

    public Vector3 HandsRotation => _handsRotation;

    private void Awake()
    {
        _colliders = GetComponentsInChildren<Collider>().ToList();
    }

    public void OnGrab()
    {
        foreach (var collider in _colliders)
        {
            collider.enabled = false;
        }
        if (_rigidbody != null)
            _rigidbody.isKinematic = true;
    }
}

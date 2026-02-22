using System;
using UnityEngine;
using Zenject;

public class RaycastItemDetector : MonoBehaviour, IItemDetector
{
    [SerializeField] private float _detectDistance;
    [Inject] private Camera _camera;
    public Transform ItemInView => _itemInView;

    public event Action<Transform> OnItemDetected;
    public event Action<Transform> OnItemMissed;

    private Transform _itemInView = null;

    private void Update()
    {
        Vector3 direction = _camera.transform.forward.normalized;
        bool isHit = Physics.Raycast(_camera.transform.position, direction,
            out RaycastHit hitInfo, _detectDistance);
        Transform itemOnPointer = null;
        if (isHit)
        {
            itemOnPointer = hitInfo.transform;
        }

        if (_itemInView != itemOnPointer)
        {
            if (_itemInView != null)
            {
                PointerExitItem(_itemInView);
                _itemInView = null;
            }
            else if (itemOnPointer != null)
            {
                _itemInView = itemOnPointer;
                OnPointerEnterItem(_itemInView);
            }
        }
    }

    private void OnPointerEnterItem(Transform item)
    {
        OnItemDetected?.Invoke(item);
    }

    private void PointerExitItem(Transform item)
    {
        OnItemMissed?.Invoke(item);
    }
}

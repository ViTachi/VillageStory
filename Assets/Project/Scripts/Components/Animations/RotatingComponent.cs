using UnityEngine;

public class RotatingComponent : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _axis;

    private void Update()
    {
        transform.Rotate(_axis * _speed * Time.deltaTime);
    }
}

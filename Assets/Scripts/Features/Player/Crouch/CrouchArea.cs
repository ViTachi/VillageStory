using UnityEngine;

public class CrouchArea : MonoBehaviour
{
    [SerializeField] private Transform _centerPoint;

    public Vector3 CenterPosition => _centerPoint.position;
}

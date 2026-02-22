using UnityEngine;

public class LocationPoint : MonoBehaviour
{
    [SerializeField] private PointType _type;

    public PointType Type => _type;

    public enum PointType
    { 
        FirstFloor, 
        SecondFloor,
        FirstFloor1,
        FirstFloor2,
        SecondFloor1,
        SecondFloor2,
        SecondFloor3,
    };

#if UNITY_EDITOR
    private void OnValidate()
    {
        gameObject.name = _type.ToString();
    }
#endif
}

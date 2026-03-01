using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    [SerializeField] private List<Transform> _points;
    [SerializeField] private List<Transform> _points2;

    public List<Transform> Points(int index) => index == 0 ? _points : _points2;
}

using System;
using UnityEngine;

public interface IItemDetector
{
    Transform ItemInView { get; }
    event Action<Transform> OnItemDetected;
    event Action<Transform> OnItemMissed;
}

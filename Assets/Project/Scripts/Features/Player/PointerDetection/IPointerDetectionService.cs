using System;
using UnityEngine;

namespace Player
{
    public interface IPointerDetectionService
    {
        Transform ItemInView { get; }
        event Action<Transform> OnItemDetected;
        event Action<Transform> OnItemMissed;
    }
}
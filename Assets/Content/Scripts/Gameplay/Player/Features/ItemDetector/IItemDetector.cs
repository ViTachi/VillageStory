using System;
using UnityEngine;

namespace Gameplay
{
    public interface IItemDetector
    {
        Transform ItemInView { get; }
        event Action<Transform> OnItemDetected;
        event Action<Transform> OnItemMissed;
    }
}

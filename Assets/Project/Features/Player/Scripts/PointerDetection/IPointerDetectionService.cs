using System;
using UnityEngine;

namespace Player
{
    public interface IPointerDetectionService
    {
        void Initialize();
        void Cleanup();
        void SetActive(bool active);
        Transform ItemInView { get; }
        event Action<Transform> OnItemDetected;
        event Action<Transform> OnItemMissed;
    }
}
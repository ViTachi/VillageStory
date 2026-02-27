using UnityEngine;

namespace Gameplay
{
    public interface IholdableItem
    {
        Transform Transform { get; }
        Vector3 HandsOffset { get; }
        Vector3 HandsRotation { get; }
        void OnGrab();
    }
}

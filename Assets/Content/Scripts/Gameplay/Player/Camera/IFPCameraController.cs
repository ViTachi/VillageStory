using UnityEngine;

namespace Gameplay
{
    public interface IFPCameraController
    {
        void LockCursor(bool locked);
        void Rotate(Vector2 delta);
    }
}
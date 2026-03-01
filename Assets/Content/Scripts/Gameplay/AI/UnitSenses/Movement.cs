using UnityEngine;

namespace Gameplay.AI
{
    public abstract class Movement : MonoBehaviour
    {
        protected bool _isMoving = false;
        protected bool _isTargetReached = false;
        protected bool _isMovementNotPossible = false;

        public bool IsMoving => _isMoving;
        public bool IsTargetReached => _isTargetReached;
        public bool IsMovementNotPossible => _isMovementNotPossible;

        public abstract void MoveToPoint(Vector3 point, float stopDistance = 0f);
        public abstract void MoveToTarget(Transform target, float stopDistance = 0f);
        public abstract float GetDistanceFromPoint(Vector3 point);
        public abstract bool IsPathReachable(Vector3 targetPoint);
        public abstract void Stop();
    }
}
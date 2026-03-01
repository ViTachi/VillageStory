using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.AI
{
    public class BaseMovement : Movement
    {
        [SerializeField] private PlayerController _controller;
        [SerializeField] private Transform characterRoot;
        [SerializeField] private Transform forward;
        [SerializeField] private Animator animator;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float visualRotationSpeed = 5f;

        [Inject] private IPathFinder _pathFinder;

        private NavMeshPath path;

        private Vector3 targetPosition;
        private Quaternion targetRotation = Quaternion.identity;
        private Transform target = null;
        private float stopDistance = 0;


        private void Awake()
        {
            path = new NavMeshPath();
        }

        private void Update()
        {
            if (_isMoving)
            {
                if (animator != null) animator.SetFloat("Speed", _controller.CurrentSpeed);

                if (targetRotation != Quaternion.identity)
                {
                    forward.rotation = Quaternion.Lerp(forward.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    characterRoot.rotation = Quaternion.Lerp(characterRoot.rotation, targetRotation, visualRotationSpeed * Time.deltaTime);
                }
            }
        }

        private void FixedUpdate()
        {
            if (_isMoving)
            {
                path = _pathFinder.GetPath(_controller.transform.position, target == null ? targetPosition : target.position, out float distance);

                if (path.status == NavMeshPathStatus.PathInvalid)
                {
                    _isMovementNotPossible = true;
                    Stop();
                    return;
                }

                if (path.corners.Length < 2 || distance <= stopDistance + _controller.CurrentSpeed * Time.fixedDeltaTime * 2)
                {
                    OnTargetReached();
                }
                else
                {
                    Vector3 direction = path.corners[1] - _controller.transform.position;
                    if (direction.magnitude < _controller.CurrentSpeed * Time.fixedDeltaTime)
                    {
                        _controller.SetPosition(path.corners[1]);
                        if (path.corners.Length < 3) OnTargetReached();
                    }
                    else
                    {
                        direction.y = 0;
                        targetRotation = Quaternion.LookRotation(direction.normalized);

                        _controller.Move(Vector3.forward);
                    }
                }
            }
        }

        private void OnTargetReached()
        {
            _isTargetReached = true;
            Stop();
        }

        public override void MoveToPoint(Vector3 point, float stopDistance = 0f)
        {
            Stop();

            _isTargetReached = false;
            _isMoving = true;
            this.stopDistance = stopDistance;
            _isMovementNotPossible = false;
            targetPosition = point;
        }

        public override void MoveToTarget(Transform target, float stopDistance = 0f)
        {
            MoveToPoint(target.position, stopDistance);
            this.target = target;
        }

        public override void Stop()
        {
            _controller.Move(Vector3.zero);
            _isMoving = false;
            if (animator != null) animator.SetFloat("Speed", 0);
            stopDistance = 0;
            target = null;
        }

        public override float GetDistanceFromPoint(Vector3 point)
        {
            _pathFinder.GetPath(_controller.transform.position, point, out float distance);
            return distance;
        }

        private void OnDrawGizmos()
        {
            if (path == null || path.corners.Length < 2) return;
            Color c = Gizmos.color;

            for (int i = 1; i < path.corners.Length; i++)
            {
                Vector3 point = path.corners[i];
                Vector3 prevPoint = path.corners[i - 1];
                Gizmos.DrawLine(prevPoint, point);
            }

            Gizmos.color = c;
        }

        public override bool IsPathReachable(Vector3 targetPoint)
        {
            var path = _pathFinder.GetPath(_controller.transform.position, targetPoint, out float distance);
            return path.status != NavMeshPathStatus.PathInvalid;
        }
    }
}
using Player;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace AI
{
    public class BaseMovement : Movement
    {
        [SerializeField] private PlayerController _controller;
        [SerializeField] private Transform _characterRoot;
        [SerializeField] private Transform _forward;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private float _visualRotationSpeed = 5f;

        [Inject] private IPathFindService _pathFinder;

        private NavMeshPath _path;

        private Vector3 _targetPosition;
        private Quaternion _targetRotation = Quaternion.identity;
        private Transform _target = null;
        private float _stopDistance = 0;


        private void Awake()
        {
            _path = new NavMeshPath();
        }

        private void Update()
        {
            if (_isMoving)
            {
                if (_animator != null) _animator.SetFloat("Speed", _controller.CurrentSpeed);

                if (_targetRotation != Quaternion.identity)
                {
                    _forward.rotation = Quaternion.Lerp(_forward.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
                    _characterRoot.rotation = Quaternion.Lerp(_characterRoot.rotation, _targetRotation, _visualRotationSpeed * Time.deltaTime);
                }
            }
        }

        private void FixedUpdate()
        {
            if (_isMoving)
            {
                _path = _pathFinder.GetPath(_controller.transform.position, _target == null ? _targetPosition : _target.position, out float distance);

                if (_path.status == NavMeshPathStatus.PathInvalid)
                {
                    _isMovementNotPossible = true;
                    Stop();
                    return;
                }

                if (_path.corners.Length < 2 || distance <= _stopDistance + _controller.CurrentSpeed * Time.fixedDeltaTime * 2)
                {
                    OnTargetReached();
                }
                else
                {
                    Vector3 direction = _path.corners[1] - _controller.transform.position;
                    if (direction.magnitude < _controller.CurrentSpeed * Time.fixedDeltaTime)
                    {
                        _controller.SetPosition(_path.corners[1]);
                        if (_path.corners.Length < 3) OnTargetReached();
                    }
                    else
                    {
                        direction.y = 0;
                        _targetRotation = Quaternion.LookRotation(direction.normalized);

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
            _stopDistance = stopDistance;
            _isMovementNotPossible = false;
            _targetPosition = point;
        }

        public override void MoveToTarget(Transform target, float stopDistance = 0f)
        {
            MoveToPoint(target.position, stopDistance);
            _target = target;
        }

        public override void Stop()
        {
            _controller.Move(Vector3.zero);
            _isMoving = false;
            if (_animator != null) _animator.SetFloat("Speed", 0);
            _stopDistance = 0;
            _target = null;
        }

        public override float GetDistanceFromPoint(Vector3 point)
        {
            _pathFinder.GetPath(_controller.transform.position, point, out float distance);
            return distance;
        }

        private void OnDrawGizmos()
        {
            if (_path == null || _path.corners.Length < 2) return;
            Color c = Gizmos.color;

            for (int i = 1; i < _path.corners.Length; i++)
            {
                Vector3 point = _path.corners[i];
                Vector3 prevPoint = _path.corners[i - 1];
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
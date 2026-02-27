using DG.Tweening;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Items
{
    public class MovedPlatform : Platform, IMoverController
    {
        [SerializeField] private PhysicsMover _mover;
        [SerializeField] private bool _moveOnAwake;
        [SerializeField] private float _speed;
        [SerializeField] private Ease _moveEase;
        [SerializeField] private bool _startMoveFromEnd;
        [SerializeField] private int _startPointIndex = 0;
        [SerializeField] private bool _loopMoving;
        [SerializeField] private List<Transform> _points;

        private int _nextPointIndex;
        private int _currentPointIndex;
        private bool _direction;
        private Tweener _moveTweener = null;
        private Coroutine _moveCoroutine = null;
        private bool _isMoving = false;

        public void ResetState()
        {
            if (_points.Count == 0) return;

            _direction = !_startMoveFromEnd;

            _currentPointIndex = _direction ? GetClampedPointIndex(_startPointIndex) : GetClampedPointIndex(_points.Count - _startPointIndex - 1);
            _nextPointIndex = _direction ? GetClampedPointIndex(_startPointIndex + 1) : GetClampedPointIndex(_points.Count - _startPointIndex - 2);
            _mover.transform.position = goalPosition = _points[_currentPointIndex].position;
        }

        [Button]
        public void StartMove()
        {
            StopMove();

            _moveCoroutine = StartCoroutine(Move());
        }

        public void SetDirection(bool direction)
        {
            if (this._direction == direction) return;
            ReverseDirection();
        }

        [Button]
        public void ReverseDirection()
        {
            _direction = !_direction;

            int cachedNextPointIndex = _nextPointIndex;
            _nextPointIndex = _currentPointIndex;
            _currentPointIndex = cachedNextPointIndex;

            if (_isMoving)
                StartMove();
        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
        {
            goalPosition = this.goalPosition;
            goalRotation = _mover.transform.rotation;
        }

        private void Awake()
        {
            ResetState();
            if (_moveOnAwake) StartMove();

            _mover.MoverController = this;
        }

        private int GetClampedPointIndex(int index) => Mathf.Clamp(index, 0, _points.Count - 1);

        [Button]
        public void StopMove()
        {
            _isMoving = false;
            _moveTweener?.Kill();
            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);
        }

        private Vector3 goalPosition;

        private IEnumerator Move()
        {
            _isMoving = true;
            while (true)
            {
                _moveTweener = DOTween.To(() => _mover.transform.position, (position) => { goalPosition = position; },
                    _points[_nextPointIndex].position, _speed).SetSpeedBased().SetEase(_moveEase).SetId(this);

                yield return _moveTweener.WaitForCompletion();

                if (IsLastPoint(_nextPointIndex))
                {
                    if (_loopMoving)
                        ReverseDirection();
                    else
                        StopMove();
                }
                else
                {
                    _currentPointIndex = _nextPointIndex;
                    _nextPointIndex += _direction ? 1 : -1;
                }
            }
        }

        private bool IsLastPoint(int pointIndex)
        {
            return _direction ? (pointIndex == (_points.Count - 1)) : pointIndex == 0;
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_points == null) return;

            Color c = Gizmos.color;
            Gizmos.color = new Color(0.2f, 1f, 0.5f, 0.7f);
            for (int i = 0; i < _points.Count; i++)
            {
                if (_points[i] == null) continue;

                Transform point = _points[i].transform;
                if (point != null)
                {
                    Gizmos.DrawSphere(point.position, 0.15f);
                    UnityEditor.Handles.Label(point.position + Vector3.up * 0.3f, $"{i + 1}");
                }
            }
            Gizmos.color = c;
        }
#endif
    }
}

using DG.Tweening;
using KinematicCharacterController;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public class MovedPlatform : Platform, IMoverController
    {
        [SerializeField] private PhysicsMover mover;
        [SerializeField] private bool moveOnAwake;
        [SerializeField] private float speed;
        [SerializeField] private Ease moveEase;
        [SerializeField] private bool startMoveFromEnd;
        [SerializeField] private int startPointIndex = 0;
        [SerializeField] private bool loopMoving;
        [SerializeField] private List<Transform> points;

        private int nextPointIndex;
        private int currentPointIndex;
        private bool direction;
        private Tweener moveTweener = null;
        private Coroutine moveCoroutine = null;
        private bool isMoving = false;

        public void ResetState()
        {
            if (points.Count == 0) return;

            direction = !startMoveFromEnd;

            currentPointIndex = direction ? GetClampedPointIndex(startPointIndex) : GetClampedPointIndex(points.Count - startPointIndex - 1);
            nextPointIndex = direction ? GetClampedPointIndex(startPointIndex + 1) : GetClampedPointIndex(points.Count - startPointIndex - 2);
            mover.transform.position = goalPosition = points[currentPointIndex].position;
        }

        [Button]
        public void StartMove()
        {
            StopMove();

            moveCoroutine = StartCoroutine(Move());
        }

        public void SetDirection(bool direction)
        {
            if (this.direction == direction) return;
            ReverseDirection();
        }

        [Button]
        public void ReverseDirection()
        {
            direction = !direction;

            int cachedNextPointIndex = nextPointIndex;
            nextPointIndex = currentPointIndex;
            currentPointIndex = cachedNextPointIndex;

            if (isMoving)
                StartMove();
        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
        {
            goalPosition = this.goalPosition;
            goalRotation = mover.transform.rotation;
        }

        private void Awake()
        {
            ResetState();
            if (moveOnAwake) StartMove();

            mover.MoverController = this;
        }

        private int GetClampedPointIndex(int index) => Mathf.Clamp(index, 0, points.Count - 1);

        [Button]
        public void StopMove()
        {
            isMoving = false;
            moveTweener?.Kill();
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);
        }

        private Vector3 goalPosition;

        private IEnumerator Move()
        {
            isMoving = true;
            while (true)
            {
                moveTweener = DOTween.To(() => mover.transform.position, (position) => { goalPosition = position; },
                    points[nextPointIndex].position, speed).SetSpeedBased().SetEase(moveEase).SetId(this);

                yield return moveTweener.WaitForCompletion();

                if (IsLastPoint(nextPointIndex))
                {
                    if (loopMoving)
                        ReverseDirection();
                    else
                        StopMove();
                }
                else
                {
                    currentPointIndex = nextPointIndex;
                    nextPointIndex += direction ? 1 : -1;
                }
            }
        }

        private bool IsLastPoint(int pointIndex)
        {
            return direction ? (pointIndex == (points.Count - 1)) : pointIndex == 0;
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (points == null) return;

            Color c = Gizmos.color;
            Gizmos.color = new Color(0.2f, 1f, 0.5f, 0.3f);
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i] == null) continue;

                Transform point = points[i].transform;
                if (point != null)
                {
                    if (i > 0)
                        Gizmos.DrawLine(points[i - 1].position, point.position);
                    Gizmos.DrawWireSphere(point.position, 0.15f);
                    UnityEditor.Handles.Label(point.position + Vector3.up * 0.3f, $"{i + 1}");
                }
            }
            Gizmos.color = c;
        }
#endif
    }
}

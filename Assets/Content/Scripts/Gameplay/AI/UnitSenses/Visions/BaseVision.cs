using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Zenject;
using Core.Extensions;
using Gameplay.Units;

namespace Gameplay.AI
{
    public class BaseVision : Vision
    {
        [SerializeField] private Transform _eyesPosition;
        [SerializeField] private Transform _visionDirection;
        [SerializeField] private LayerMask _detectLayerMask;
        [SerializeField] private float _visionFieldSize = 180f;
        [SerializeField] private float _feelDistance = 1f;

        [Inject] private Unit _unit;

        private List<Unit> lostUnits = new List<Unit>();

        private Vector3 Position => _eyesPosition.position;
        private Vector3 Direction => _visionDirection.forward;

        private void Update()
        {
            Collider[] colliders = Physics.OverlapSphere(Position, _detectDistance, _detectLayerMask);

            lostUnits.Clear();
            foreach (var unit in UnitsInVision)
            {
                if (unit == null || !colliders.Any(x => x.transform == unit.transform)
                    || !IsUnitInVisionField(unit) || IsUnitBehindTheObject(unit))
                    lostUnits.Add(unit);
            }

            foreach (var unit in lostUnits)
            {
                if (!IsUnitHiden(unit))
                    UnitLost(unit);
            }

            for (int i = 0; i < colliders.Length; i++)
            {
                Unit unit = colliders[i].GetComponent<Unit>();
                if (unit != null && unit != GetComponentInParent<Unit>()
                    && IsUnitInVisionField(unit) && !IsUnitBehindTheObject(unit) && !IsUnitHiden(unit))
                {
                    UnitDetected(unit);
                }
            }
        }

        private bool IsUnitHiden(Unit unit)
        {
            if (unit is ICanHide canHide)
            {
                return canHide.IsHiden;
            }
            return false;
        }

        private bool IsUnitInVisionField(Unit aIUnit)
        {
            if ((aIUnit.Position - Position).sqrMagnitude
                <= Mathf.Pow(_feelDistance + aIUnit.Radius + _unit.Radius, 2))
                return true;

            if ((aIUnit.Position - Position).sqrMagnitude > _detectDistance * _detectDistance)
                return false;

            if (Vector3.Dot(Direction, (aIUnit.Position - Position).normalized)
                   < _visionFieldSize.Remap(0f, 360f, 1, -1f))
                return false;
            return true;
        }

        private bool IsUnitBehindTheObject(Unit aIUnit)
        {
            Vector3[] points = GetPointsOfBounds(aIUnit.Bounds);
            foreach (var point in points)
            {
                if (!IsUnitPartBehindTheObject(point, aIUnit)) return false;
            }
            return true;
        }

        private Vector3[] GetPointsOfBounds(Bounds bounds)
        {
            Vector3 center = bounds.center + Vector3.up * bounds.size.y * 0.2f;
            float offsetX = bounds.size.x * 0.3f;
            float offsetZ = bounds.size.z * 0.3f;
            Vector3[] points =
            {
                bounds.center,
                new Vector3(center.x + offsetX, center.y, center.z),
                new Vector3(center.x - offsetX, center.y, center.z),
                new Vector3(center.x, center.y, center.z + offsetZ),
                new Vector3(center.x, center.y, center.z - offsetZ)
            };
            return points;
        }

        private bool TryGetUnit(RaycastHit[] hits, out Unit aIUnit)
        {
            aIUnit = null;
            RaycastHit hit = hits.Where(x => x.transform != _unit.transform &&
                !x.transform.IsChildOf(_unit.transform))
                .OrderBy(x => (x.point - Position).sqrMagnitude)
                .FirstOrDefault();
            if (hit.transform != null)
            {
                aIUnit = hit.transform.GetComponentInParent<Unit>();
            }
            return aIUnit != null;
        }

        private bool IsUnitPartBehindTheObject(Vector3 point, Unit aIUnit)
        {
            RaycastHit[] allHits = Physics.RaycastAll(Position, (point - Position).normalized, _detectDistance);

            if (TryGetUnit(allHits, out Unit detectedAIUnit))
            {
                return detectedAIUnit != aIUnit;
            }
            return true;
        }

        private void OnDrawGizmos()
        {
            Color c = Gizmos.color;
            Gizmos.color = Color.green;
            foreach (var aIUnit in UnitsInVision)
            {
                Vector3[] points = GetPointsOfBounds(aIUnit.Bounds);

                bool isVisible = false;
                foreach (var point in points)
                {
                    if (IsUnitPartBehindTheObject(point, aIUnit))
                        Gizmos.color = Color.red;
                    else
                    {
                        Gizmos.color = Color.green;
                        isVisible = true;
                    }
                    Gizmos.DrawLine(Position, point);
                }
                Gizmos.color = isVisible ? Color.green : Color.red;
                Gizmos.DrawWireCube(aIUnit.Bounds.center, aIUnit.Bounds.size);
            }
            Gizmos.color = c;
        }
    }
}
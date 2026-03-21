using System;
using System.Collections.Generic;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.AI
{
    public abstract class Vision : MonoBehaviour
    {
        [SerializeField] protected float _detectDistance = 10f;
        [SerializeField] private List<Unit> _unitsInVision = new List<Unit>();

        public IReadOnlyList<Unit> UnitsInVision => _unitsInVision;

        public event Action<Unit> OnUnitDetected;
        public event Action<Unit> OnUnitLost;


        private void OnDisable()
        {
            _unitsInVision.Clear();
        }

        protected bool IsAnyUnitsInVision() => _unitsInVision.Count > 0;

        protected void UnitDetected(Unit aIUnit)
        {
            if (_unitsInVision.Contains(aIUnit)) return;

            _unitsInVision.Add(aIUnit);
            OnUnitDetected?.Invoke(aIUnit);
        }

        protected void UnitLost(Unit aIUnit)
        {
            if (!_unitsInVision.Contains(aIUnit)) return;

            _unitsInVision.Remove(aIUnit);
            OnUnitLost?.Invoke(aIUnit);
        }
    }
}
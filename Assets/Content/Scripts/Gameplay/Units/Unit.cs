using System;
using UnityEngine;

namespace Gameplay
{
    public abstract class Unit : MonoBehaviour
    {
        private bool _isAlive = true;
        public bool IsAlive => _isAlive;
        public event Action OnDie;

        public abstract Vector3 Position { get; }
        public abstract Vector3 HeadPosition { get; }

        public abstract Bounds Bounds { get; }
        public abstract float Radius { get; }
        public abstract bool IsHiden { get; }
        public abstract Vector3 ForwardDirection { get; }
        public abstract float GetDistanceFromPoint(Vector3 point);
        public abstract void SetControlActive(bool active);
        public virtual void Die()
        {
            OnDie?.Invoke();
            _isAlive = false;
        }
    }
}

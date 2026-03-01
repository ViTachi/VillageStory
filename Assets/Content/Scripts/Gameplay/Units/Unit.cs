using System;
using UnityEngine;

namespace Gameplay
{
    public class Unit : MonoBehaviour
    {
        private bool _isAlive = true;
        public bool IsAlive => _isAlive;
        public event Action OnDie;

        public virtual Vector3 Position { get; }
        public virtual Vector3 HeadPosition { get; }

        public virtual Bounds Bounds { get; }
        public virtual float Radius { get; }
        public virtual Vector3 ForwardDirection { get; }

        public virtual void Die()
        {
            OnDie?.Invoke();
            _isAlive = false;
        }
    }
}

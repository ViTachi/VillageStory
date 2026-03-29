using System;

namespace Units
{
    public interface ICanDie
    {
        public bool IsAlive { get; }
        public event Action OnDie;
        void Die();
    }
}
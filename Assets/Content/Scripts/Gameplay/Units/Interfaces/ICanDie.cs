using System;

namespace Gameplay.Units
{
    public interface ICanDie
    {
        public bool IsAlive { get; }
        public event Action OnDie;
        void Die();
    }
}
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using Zenject;

namespace Gameplay.AI
{
    public abstract class Intention : MonoBehaviour
    {
        [SerializeField] private int _priority = 0;

        [Inject] private UnitActionsLauncher _unitActionsLauncher;

        public int Priority => _currentPriority;
        public bool IsCompleted => _isCompleted;
        public bool IsRunning => _isRunning;

        public event Action<Intention> OnActivate;
        public event Action<Intention> OnComplete;
        public event Action<Intention> OnStop;

        protected Intention _rootIntention;
        private int _currentPriority = 0;
        private bool _isCompleted = false;
        private bool _isRunning = false;

        public virtual void Initialize(Intention rootIntention = null)
        {
            _currentPriority = _priority;
            _rootIntention = rootIntention;
        }
        public virtual void Activate()
        {
            _isCompleted = false;
            _isRunning = true;
            OnActivate?.Invoke(this);

#if UNITY_EDITOR
            gameObject.name = $"{gameObject.name}(Active)";
#endif
        }

        public void SetPriority(int priority) => _currentPriority = priority;
        public void ResetPriority() => _currentPriority = _priority;
        public abstract bool IsNeedToBeActivated();
        public virtual void Run() { }

        [Button]
        public virtual void Stop()
        {
            _isRunning = false;
            ResetPriority();
            OnStop?.Invoke(this);
            _unitActionsLauncher.StopAllActions();

#if UNITY_EDITOR
            gameObject.name = gameObject.name.Replace("(Active)", string.Empty);
#endif
        }

        public virtual void Complete()
        {
            Stop();
            _isCompleted = true;
            OnComplete?.Invoke(this);
        }

        protected virtual void OnDisable()
        {
            if (_isRunning)
                Stop();
        }
    }
}
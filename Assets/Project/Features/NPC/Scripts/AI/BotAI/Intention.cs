using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace AI
{
    public abstract class Intention : MonoBehaviour
    {
        [SerializeField] private int _priority = 0;

        [Inject] protected UnitActionsLauncher _unitActionsLauncher;

        public int Priority => _currentPriority;
        public bool IsCompleted => _isCompleted;
        public bool IsRunning => _isRunning;

        public event Action<Intention> OnActivate;
        public event Action<Intention> OnComplete;
        public event Action<Intention> OnStop;

        private int _currentPriority;
        private bool _isCompleted = false;
        private bool _isRunning = false;

        private CancellationTokenSource _cts;

        public virtual void Initialize(Intention rootIntention = null)
        {
            _currentPriority = _priority;
        }

        public virtual void Activate()
        {
            if (_isRunning) return;

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();

            _isCompleted = false;
            _isRunning = true;
            OnActivate?.Invoke(this);

#if UNITY_EDITOR
            gameObject.name = $"{gameObject.name}(Active)";
#endif
            RunAsync(_cts.Token).Forget();
        }

        public abstract bool IsNeedToBeActivated();
        protected abstract UniTask RunAsync(CancellationToken token);

        public void SetPriority(int priority) => _currentPriority = priority;
        public void ResetPriority() => _currentPriority = _priority;

        public virtual void Stop()
        {
            if (!_isRunning) return;

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;

            _isRunning = false;
            ResetPriority();
            OnStop?.Invoke(this);

#if UNITY_EDITOR
            try
            {
                gameObject.name = gameObject.name.Replace("(Active)", string.Empty);
            }
            catch (MissingReferenceException) { }
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
            // if (_isRunning)
            //     Stop();
        }
    }
}
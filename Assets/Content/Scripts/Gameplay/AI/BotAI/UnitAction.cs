using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.AI
{
    public abstract class UnitAction<T> : UnitActionBase
    {
        public event Action<UnitAction<T>> OnStart;
        public event Action<UnitAction<T>> OnStop;
        public event Action<UnitAction<T>> OnComplete;

        protected T _parameters;

        public async virtual UniTask StartAction(CancellationToken token, T paramters = default)
        {
            if (_isInProgress)
                StopAction();

            _parameters = paramters;

            _isInProgress = true;
            OnStart?.Invoke(this);

#if UNITY_EDITOR
            gameObject.name = $"{gameObject.name}(Active)";
#endif

            try
            {
                await RunAction(token);
                CompleteAction();
            }
            catch (OperationCanceledException)
            {
                StopAction();
            }
        }

        protected abstract UniTask RunAction(CancellationToken token);

        public virtual void StopAction()
        {
            if (!_isInProgress) return;

            _isInProgress = false;
            OnStop?.Invoke(this);

#if UNITY_EDITOR
            gameObject.name = gameObject.name.Replace("(Active)", string.Empty);
#endif
        }

        public virtual void CompleteAction()
        {
            StopAction();
            OnComplete?.Invoke(this);
        }
    }

    public abstract class UnitActionBase : MonoBehaviour
    {
        protected bool _isInProgress = false;
        public bool IsInProgress => _isInProgress;
    }
}
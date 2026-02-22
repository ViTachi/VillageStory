using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Presentation.UI
{
    public abstract class Screen<TParams, TResult> : MonoBehaviour
    {
        private Action<TResult> _callback;
        protected TResult _result;

        public virtual UniTask Show(TParams parameters, bool immediately,
        CancellationToken ct, Action<TResult> callback = null)
        {
            _callback = callback;
            return UniTask.CompletedTask;
        }

        public virtual UniTask Hide(bool immediately, CancellationToken ct)
        {
            _callback?.Invoke(_result);
            return UniTask.CompletedTask;
        }
    }
}
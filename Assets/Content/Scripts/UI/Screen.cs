using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.UI
{
    public abstract class Screen<TParams, TResult> : UIElement
    {
        private Action<TResult> _callback;
        protected TResult _result;

        public virtual UniTask Show(TParams parameters, bool immediately,
            CancellationToken ct, Action<TResult> callback = null)
        {
            base.Show(immediately, ct);
            _callback = callback;
            return UniTask.CompletedTask;
        }

        public override UniTask Hide(bool immediately, CancellationToken ct)
        {
            base.Hide(immediately, ct);
            _callback?.Invoke(_result);
            return UniTask.CompletedTask;
        }
    }
}
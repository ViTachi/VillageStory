using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace UI
{
    public abstract class Screen<TParams, TResult> : UIElement
    {
        private Action<TResult> callback;
        protected TResult result;

        public virtual UniTask Show(TParams parameters, bool immediately,
            CancellationToken ct, Action<TResult> callback = null)
        {
            base.Show(immediately, ct);
            this.callback = callback;
            return UniTask.CompletedTask;
        }

        public override UniTask Hide(bool immediately, CancellationToken ct)
        {
            base.Hide(immediately, ct);
            callback?.Invoke(result);
            return UniTask.CompletedTask;
        }
    }
}
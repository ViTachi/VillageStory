using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace UI
{
    public interface IUIService
    {
        UniTask<T> ShowScreen<T, TParams, TResult>(UILayer layer, TParams parameters,
            bool immediately, CancellationToken ct, Action<TResult> callback) where T : Screen<TParams, TResult>;

        UniTask HideScreen<T>(bool immediately, CancellationToken ct)
            where T : UIElement;

        T GetScreen<T, TParams, TResult>() where T : Screen<TParams, TResult>;
    }
}
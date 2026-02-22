using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Presentation.UI;

namespace Services
{
    public interface IUIService
    {
        UniTask ShowScreen<T, TParams, TResult>(UILayer layer, TParams parameters,
            bool immediately, CancellationToken ct, Action<TResult> callback) where T : Screen<TParams, TResult>;

        UniTask HideScreen<T, TParams, TResult>(bool immediately, CancellationToken ct)
            where T : Screen<TParams, TResult>;
    }
}
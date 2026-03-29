using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace UI
{
    public class UIService : MonoBehaviour, IUIService, IInitializable
    {
        [SerializeField] private Transform _mainLayer;
        [SerializeField] private Transform _popupLayer;
        [SerializeField] private Transform _notificationLayer;

        private Dictionary<Type, ScreenData> _screens = new Dictionary<Type, ScreenData>();

        private struct ScreenData
        {
            public UIElement Screen;
            public AsyncOperationHandle Handle;
        }

        public void Initialize()
        {

        }

        public async UniTask<T> ShowScreen<T, TParams, TResult>(UILayer layer, TParams parameters,
            bool immediately, CancellationToken ct, Action<TResult> callback)
            where T : Screen<TParams, TResult>
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(typeof(T).Name);
            await handle;
            GameObject screenPrefab = handle.Result;
            Transform parent = _mainLayer;
            switch (layer)
            {
                case UILayer.Main:
                    parent = _mainLayer;
                    break;
                case UILayer.Notification:
                    parent = _popupLayer;
                    break;
                case UILayer.Popup:
                    parent = _notificationLayer;
                    break;
            }

            var screenInstance = Instantiate(screenPrefab, parent);

            if (screenInstance.TryGetComponent(out Screen<TParams, TResult> screen))
            {
                _screens[typeof(T)] = new ScreenData() { Screen = screen, Handle = handle };
                await screen.Show(parameters, immediately, ct, callback);
                return screen as T;
            }

            return null;
        }

        public async UniTask HideScreen<T>(bool immediately, CancellationToken ct)
            where T : UIElement
        {
            if (_screens.TryGetValue(typeof(T), out ScreenData screenData))
            {
                await screenData.Screen.Hide(immediately, ct);
                Addressables.Release(screenData.Handle);
                _screens.Remove(typeof(T));
            }
        }

        public T GetScreen<T, TParams, TResult>() where T : Screen<TParams, TResult>
        {
            if (_screens.TryGetValue(typeof(T), out ScreenData screenData))
            {
                return screenData.Screen as T;
            }
            return default;
        }
    }
}
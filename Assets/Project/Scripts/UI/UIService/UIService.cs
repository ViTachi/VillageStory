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
        [SerializeField] private Transform mainLayer;
        [SerializeField] private Transform popupLayer;
        [SerializeField] private Transform notificationLayer;

        private Dictionary<Type, ScreenData> screens = new Dictionary<Type, ScreenData>();

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
            Transform parent = mainLayer;
            switch (layer)
            {
                case UILayer.Main:
                    parent = mainLayer;
                    break;
                case UILayer.Notification:
                    parent = popupLayer;
                    break;
                case UILayer.Popup:
                    parent = notificationLayer;
                    break;
            }

            var screenInstance = Instantiate(screenPrefab, parent);

            if (screenInstance.TryGetComponent(out Screen<TParams, TResult> screen))
            {
                screens[typeof(T)] = new ScreenData() { Screen = screen, Handle = handle };
                await screen.Show(parameters, immediately, ct, callback);
                return screen as T;
            }

            return null;
        }

        public async UniTask HideScreen<T>(bool immediately, CancellationToken ct)
            where T : UIElement
        {
            if (screens.TryGetValue(typeof(T), out ScreenData screenData))
            {
                await screenData.Screen.Hide(immediately, ct);
                Addressables.Release(screenData.Handle);
                screens.Remove(typeof(T));
            }
        }

        public T GetScreen<T, TParams, TResult>() where T : Screen<TParams, TResult>
        {
            if (screens.TryGetValue(typeof(T), out ScreenData screenData))
            {
                return screenData.Screen as T;
            }
            return default;
        }
    }
}
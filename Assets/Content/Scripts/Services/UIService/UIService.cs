using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Presentation.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Services
{
    public class UIService : MonoBehaviour, IUIService, IInitializable
    {
        [SerializeField] private Transform _mainLayer;
        [SerializeField] private Transform _popupLayer;
        [SerializeField] private Transform _notificationLayer;

        private Dictionary<Type, ScreenData> _screens = new Dictionary<Type, ScreenData>();

        private struct ScreenData
        {
            public GameObject ScreenInstance;
            public AsyncOperationHandle Handle;
        }

        public void Initialize()
        {

        }

        public async UniTask ShowScreen<T, TParams, TResult>(UILayer layer, TParams parameters,
            bool immediately, CancellationToken ct, Action<TResult> callback) where T : Screen<TParams, TResult>
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
            _screens[typeof(T)] = new ScreenData() { ScreenInstance = screenInstance, Handle = handle };

            if (screenInstance.TryGetComponent(out Screen<TParams, TResult> screen))
            {
                await screen.Show(parameters, immediately, ct, callback);
            }
        }

        public async UniTask HideScreen<T, TParams, TResult>(bool immediately, CancellationToken ct)
            where T : Screen<TParams, TResult>
        {
            if (_screens.TryGetValue(typeof(T), out ScreenData screenData))
            {
                if (screenData.ScreenInstance.TryGetComponent(out T screen))
                {
                    await screen.Hide(immediately, ct);
                    Destroy(screenData.ScreenInstance);
                    Addressables.Release(screenData.Handle);
                    _screens.Remove(typeof(T));
                }
            }
        }

    }


}
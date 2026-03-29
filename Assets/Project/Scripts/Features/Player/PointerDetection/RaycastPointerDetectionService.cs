using System;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Player
{
    public class RaycastPointerDetectionService : IPointerDetectionService, ITickable
    {
        [Inject] private CinemachineBrain _cinemachineBrain;
        [Inject] private PlayerDetectionConfig _playerDetectionConfig;
        public Transform ItemInView => _itemInView;

        public event Action<Transform> OnItemDetected;
        public event Action<Transform> OnItemMissed;

        private Transform _itemInView = null;
        private bool _active = true;

        public void Initialize()
        {
            SetActive(true);
        }

        public void Cleanup()
        {
            SetActive(false);
        }

        public void SetActive(bool active)
        {
            _active = active;
            if (!active)
            {
                if (_itemInView != null)
                    PointerExitItem(_itemInView);
            }
        }

        public void Tick()
        {
            if (!_active) return;

            Vector3 direction = _cinemachineBrain.transform.forward.normalized;
            bool isHit = Physics.Raycast(_cinemachineBrain.transform.position, direction,
                out RaycastHit hitInfo, _playerDetectionConfig.DetectDistance);
            Transform itemOnPointer = null;
            if (isHit)
            {
                itemOnPointer = hitInfo.transform;
            }


            if (_itemInView != itemOnPointer)
            {
                if (_itemInView != null)
                {
                    PointerExitItem(_itemInView);
                }

                _itemInView = itemOnPointer;

                if (_itemInView != null)
                {
                    OnPointerEnterItem(_itemInView);
                }
            }
        }

        private void OnPointerEnterItem(Transform item)
        {
            OnItemDetected?.Invoke(item);
        }

        private void PointerExitItem(Transform item)
        {
            OnItemMissed?.Invoke(item);
            _itemInView = null;
        }
    }
}
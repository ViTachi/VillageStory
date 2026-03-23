using System;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Player
{
    public class RaycastPointerDetectionService : IPointerDetectionService, ITickable
    {
        [Inject] private CinemachineBrain cinemachineBrain;
        [Inject] private PlayerDetectionConfig playerDetectionConfig;
        public Transform ItemInView => itemInView;

        public event Action<Transform> OnItemDetected;
        public event Action<Transform> OnItemMissed;

        private Transform itemInView = null;

        public void Tick()
        {
            Vector3 direction = cinemachineBrain.transform.forward.normalized;
            bool isHit = Physics.Raycast(cinemachineBrain.transform.position, direction,
                out RaycastHit hitInfo, playerDetectionConfig.DetectDistance);
            Transform itemOnPointer = null;
            if (isHit)
            {
                itemOnPointer = hitInfo.transform;
            }


            if (itemInView != itemOnPointer)
            {
                if (itemInView != null)
                {
                    PointerExitItem(itemInView);
                }

                itemInView = itemOnPointer;

                if (itemInView != null)
                {
                    OnPointerEnterItem(itemInView);
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
        }
    }
}
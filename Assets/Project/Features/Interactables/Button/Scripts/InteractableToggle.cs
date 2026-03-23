using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables
{
    public class InteractableToggle : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool on;
        [SerializeField] private UnityEvent onEnable;
        [SerializeField] private UnityEvent onDisable;

        private void Start()
        {
            Apply();
        }

        public void Interact()
        {
            on = !on;
            Apply();
        }

        private void Apply()
        {
            if (on) onEnable?.Invoke();
            else onDisable?.Invoke();
        }
    }
}
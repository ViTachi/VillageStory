using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables
{
    public class InteractableButton : MonoBehaviour, IInteractable
    {
        [SerializeField] private UnityEvent onPress;

        public void Interact()
        {
            onPress?.Invoke();
        }
    }
}
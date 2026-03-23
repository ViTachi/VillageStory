using Player;
using UnityEngine;

namespace Interactables
{
    public class DoorSide : MonoBehaviour, IInteractable
    {
        [SerializeField] private float offset;
        [SerializeField] private Door door;

        public void Interact()
        {
            if (door.IsOpened)
            {
                door.Close();
            }
            else
            {
                door.Open(offset);
            }
        }
    }
}
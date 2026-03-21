using UnityEngine;

namespace Gameplay.Items
{
    public class DoorSide : InteractableItem
    {
        [SerializeField] private float _offset;
        [SerializeField] private Door _door;

        public override void Interact(IInteractionContext context)
        {
            base.Interact(context);
            if (_door.IsOpened)
            {
                _door.Close();
            }
            else
            {
                _door.Open(_offset);
            }
        }
    }
}
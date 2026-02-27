using UnityEngine;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace Gameplay
{
    public class PlayerMoveInput : MonoBehaviour
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private FPCameraController _fPCameraController;
        [Inject] private Interactor _interactor;
        [Inject] private ItemsCollector _itemsCollector;
        [Inject] private ItemsHolder _itemsHolder;


        public void OnMove(CallbackContext callbackContext)
        {
            Vector3 inputVector = callbackContext.ReadValue<Vector2>();
            _playerController.Move(new Vector3(inputVector.x, 0f, inputVector.y));
        }

        public void OnJump(CallbackContext callbackContext)
        {
            if (callbackContext.ReadValueAsButton())
            {
                _playerController.Jump();
            }
        }

        public void OnCrouch(CallbackContext callbackContext)
        {
            if (callbackContext.started)
            {
                _playerController.SetCrouch(true);
            }
            if (callbackContext.canceled)
            {
                _playerController.SetCrouch(false);
            }
        }

        public void OnMouseMove(CallbackContext callbackContext)
           => _fPCameraController.Rotate(callbackContext.ReadValue<Vector2>());

        public void OnInteract(CallbackContext callbackContext)
        {
            if (callbackContext.ReadValueAsButton())
            {
                _interactor.OnInteract();
            }
        }

        public void OnCollect(CallbackContext callbackContext)
        {
            if (callbackContext.ReadValueAsButton())
            {
                _itemsCollector.OnCollect();
            }
        }

        public void OnUseItem(CallbackContext callbackContext)
        {
            if (callbackContext.ReadValueAsButton())
            {
                _itemsHolder.OnUseItem();
            }
        }
    }
}

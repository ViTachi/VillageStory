using Input;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public class PlayerMovementService : IPlayerMovementService, ITickable
    {
        [Inject] private PlayerMovementConfig _playerMovementConfig;
        [Inject] private IPlayerController _playerController;
        [Inject] private PlayerInput _playerInput;
        [Inject] private PlayerPresenter _playerPresenter;
        [Inject] private CinemachineCamera _cinemachineCamera;

        private Vector2 _currentRotation;
        private Vector2 _targetRotation;
        private Vector2 _inputDelta = Vector2.zero;

        public void Initialize()
        {
            _playerController.SetMoveSpeed(_playerMovementConfig.MoveSpeed);
            _playerController.SetJumpUpSpeed(_playerMovementConfig.JumpUpSpeed);
            _cinemachineCamera.Target.TrackingTarget = _playerPresenter.CameraAnchor;

            _playerInput.Player.Move.performed += Move;
            _playerInput.Player.Move.canceled += Move;
            _playerInput.Player.Jump.started += Jump;
            _playerInput.Player.Crouch.started += StartCrouch;
            _playerInput.Player.Crouch.canceled += CancelCrouch;
            _playerInput.Player.Look.performed += OnMouseMove;
            _playerInput.Player.Look.canceled += OnMouseMove;
        }

        public void Cleanup()
        {
            _playerInput.Player.Move.performed -= Move;
            _playerInput.Player.Move.canceled -= Move;
            _playerInput.Player.Jump.started -= Jump;
            _playerInput.Player.Crouch.started -= StartCrouch;
            _playerInput.Player.Crouch.canceled -= CancelCrouch;
            _playerInput.Player.Look.performed -= OnMouseMove;
            _playerInput.Player.Look.canceled -= OnMouseMove;
        }

        private void Move(CallbackContext callbackContext)
        {
            Vector3 inputVector = callbackContext.ReadValue<Vector2>();
            _playerController.Move(new Vector3(inputVector.x, 0f, inputVector.y));
        }

        private void Jump(CallbackContext callbackContext)
        {
            _playerController.Jump();
        }

        private void StartCrouch(CallbackContext callbackContext)
        {
            _playerController.SetCrouch(true);
        }

        private void CancelCrouch(CallbackContext callbackContext)
        {
            _playerController.SetCrouch(false);
        }

        public void OnMouseMove(CallbackContext callbackContext)
        {
            Rotate(callbackContext.ReadValue<Vector2>());
        }

        public void Tick()
        {
            CameraRotation();
        }

        private void Rotate(Vector2 delta) => _inputDelta = delta * _playerMovementConfig.Sensitivity * 1.0f
            * (Display.main.systemWidth / (float)Screen.currentResolution.width);

        private void CameraRotation()
        {
            if (_inputDelta.sqrMagnitude < _playerMovementConfig.Threshold)
                _inputDelta = Vector2.zero;

            _targetRotation.x -= _inputDelta.y;
            _targetRotation.y += _inputDelta.x;

            ClampAngles();

            _currentRotation = Vector2.Lerp(_currentRotation, _targetRotation, _playerMovementConfig.Interpolation * Time.deltaTime);

            _playerPresenter.CameraAnchor.localRotation = Quaternion.Euler(_currentRotation.x, _currentRotation.y, 0.0f);

            _inputDelta = Vector2.zero;
        }

        private void ClampAngles()
        {
            if (_targetRotation.x < -360f)
            {
                _targetRotation.x += 360f;
                _currentRotation.x += 360f;
            }
            if (_targetRotation.y < -360f)
            {
                _targetRotation.y += 360f;
                _currentRotation.y += 360f;
            }
            if (_targetRotation.x > 360f)
            {
                _targetRotation.x -= 360f;
                _currentRotation.x -= 360f;
            }
            if (_targetRotation.y > 360f)
            {
                _targetRotation.y -= 360f;
                _currentRotation.y -= 360f;
            }

            _targetRotation.x = Mathf.Clamp(_targetRotation.x, _playerMovementConfig.BottomClamp,
            _playerMovementConfig.TopClamp);
        }
    }
}
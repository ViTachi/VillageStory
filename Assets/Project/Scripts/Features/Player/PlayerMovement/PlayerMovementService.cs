using System;
using Gameplay;
using Input;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public class PlayerMovementService : IPlayerMovementService, ITickable, IInitializable, IDisposable
    {
        [Inject] private PlayerMovementConfig playerMovementConfig;
        [Inject] private IPlayerController playerController;
        [Inject] private PlayerInput playerInput;
        [Inject] private PlayerPresenter playerPresenter;
        [Inject] private CinemachineCamera cinemachineCamera;

        private Vector2 _currentRotation;
        private Vector2 _targetRotation;
        private Vector2 _inputDelta = Vector2.zero;

        public void Initialize()
        {
            playerController.SetMoveSpeed(playerMovementConfig.MoveSpeed);
            playerController.SetJumpUpSpeed(playerMovementConfig.JumpUpSpeed);
            cinemachineCamera.Target.TrackingTarget = playerPresenter.CameraAnchor;

            playerInput.Player.Move.performed += Move;
            playerInput.Player.Move.canceled += Move;
            playerInput.Player.Jump.started += Jump;
            playerInput.Player.Crouch.started += StartCrouch;
            playerInput.Player.Crouch.canceled += CancelCrouch;
            playerInput.Player.Look.performed += OnMouseMove;
            playerInput.Player.Look.canceled += OnMouseMove;
        }

        public void Dispose()
        {
            playerInput.Player.Move.performed -= Move;
            playerInput.Player.Move.canceled -= Move;
            playerInput.Player.Jump.started -= Jump;
            playerInput.Player.Crouch.started -= StartCrouch;
            playerInput.Player.Crouch.canceled -= CancelCrouch;
            playerInput.Player.Look.performed -= OnMouseMove;
            playerInput.Player.Look.canceled -= OnMouseMove;
        }

        private void Move(CallbackContext callbackContext)
        {
            Vector3 inputVector = callbackContext.ReadValue<Vector2>();
            playerController.Move(new Vector3(inputVector.x, 0f, inputVector.y));
        }

        private void Jump(CallbackContext callbackContext)
        {
            playerController.Jump();
        }

        private void StartCrouch(CallbackContext callbackContext)
        {
            playerController.SetCrouch(true);
        }

        private void CancelCrouch(CallbackContext callbackContext)
        {
            playerController.SetCrouch(false);
        }

        public void OnMouseMove(CallbackContext callbackContext)
           => Rotate(callbackContext.ReadValue<Vector2>());

        public void Tick()
        {
            CameraRotation();
        }

        public void Rotate(Vector2 delta) => _inputDelta = delta * playerMovementConfig.Sensitivity * 1.0f
            * (Display.main.systemWidth / (float)Screen.currentResolution.width);

        private void CameraRotation()
        {
            if (_inputDelta.sqrMagnitude < playerMovementConfig.Threshold)
                _inputDelta = Vector2.zero;

            _targetRotation.x -= _inputDelta.y;
            _targetRotation.y += _inputDelta.x;

            ClampAngles();

            _currentRotation = Vector2.Lerp(_currentRotation, _targetRotation, playerMovementConfig.Interpolation * Time.deltaTime);

            playerPresenter.CameraAnchor.localRotation = Quaternion.Euler(_currentRotation.x, _currentRotation.y, 0.0f);

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

            _targetRotation.x = Mathf.Clamp(_targetRotation.x, playerMovementConfig.BottomClamp,
            playerMovementConfig.TopClamp);
        }
    }
}
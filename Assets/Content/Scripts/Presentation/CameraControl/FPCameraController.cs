using UnityEngine;
using Zenject;

public class FPCameraController : MonoBehaviour
{
    [SerializeField] private Transform _cinemachineCameraTarget;
    [SerializeField] private float _interpolation = 10f;
    [SerializeField] private float _threshold = 0.01f;
    [SerializeField] private float _sensitivity = 1f;
    [SerializeField] private float _topClamp = 80.0f;
    [SerializeField] private float _bottomClamp = -80.0f;

    [Inject] private GameSettingsData _settings;

    private Vector2 _currentRotation;
    private Vector2 _targetRotation;
    private Vector2 _inputDelta = Vector2.zero;

    public void Rotate(Vector2 delta) => _inputDelta = delta * _sensitivity * _settings.Sensitivity
        * (Display.main.systemWidth / (float)Screen.currentResolution.width);

    public void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

    private void OnEnable()
    {
        LockCursor(true);
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        if (_inputDelta.sqrMagnitude < _threshold)
            _inputDelta = Vector2.zero;

        _targetRotation.x -= _inputDelta.y;
        _targetRotation.y += _inputDelta.x;

        ClampAngles();

        _currentRotation = Vector2.Lerp(_currentRotation, _targetRotation, _interpolation * Time.deltaTime);

        _cinemachineCameraTarget.localRotation = Quaternion.Euler(_currentRotation.x, _currentRotation.y, 0.0f);

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

        _targetRotation.x = Mathf.Clamp(_targetRotation.x, _bottomClamp, _topClamp);
    }
}

using System;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerCameraEffects : MonoBehaviour
    {
        [field: Header("Tilt")]
        [field: SerializeField] public bool TiltEnabled { get; private set; } = true;
        [field: SerializeField] public float Pitch { get; private set; } = 0.1f;
        [field: SerializeField] public float Roll { get; private set; } = 0.25f;
        [field: SerializeField] public float MaxPitch { get; private set; } = 1.0f;
        [field: SerializeField] public float MaxRoll { get; private set; } = 2.5f;
        [field: Header("Fall Kick")]
        [field: SerializeField] public bool FallKickEnabled { get; private set; } = true;
        [field: SerializeField] public float FallTime { get; private set; } = 0.3f;
        [field: SerializeField] public float FallVelocityThreshold { get; private set; } = 5.0f;
        [field: Header("Headbob")]
        [field: SerializeField] public bool HeadbobEnabled { get; private set; } = true;
        [field: SerializeField] public float HeadbobPitch { get; private set; } = 0.05f;
        [field: SerializeField] public float HeadbobRoll { get; private set; } = 0.025f;
        [field: SerializeField] public float HeadbobUp { get; private set; } = 0.05f;
        [field: SerializeField] public float HeadbobFrequency { get; private set; } = 6.0f;


        [Inject] public IPlayerController _playerController { get; private set; }

        private float _fallValue;
        private float _fallTimer;
        private float _stepTimer;

        private Vector3 _initialLocalPosition;

        private void Start()
        {
            _initialLocalPosition = transform.localPosition;

            if (_playerController != null)
                _playerController.OnLandedEvent += AddFallKick;
        }

        private void OnDestroy()
        {
            if (_playerController != null)
                _playerController.OnLandedEvent -= AddFallKick;
        }

        private void LateUpdate()
        {
            CalculateCameraOffset(Time.deltaTime);
        }

        private void CalculateCameraOffset(float delta)
        {
            _fallTimer -= delta;

            Vector3 velocity = _playerController.Velocity;

            float speed = new Vector2(velocity.x, velocity.z).magnitude;

            if (speed > 0f && _playerController.IsGrounded)
            {
                _stepTimer += delta * (speed / HeadbobFrequency);
                _stepTimer %= 1f;
            }
            else
            {
                _stepTimer = 0f;
            }

            float headbobSin =
                Mathf.Sin(_stepTimer * 2f * Mathf.PI) * 0.5f;

            Vector3 angles = Vector3.zero;
            Vector3 offset = Vector3.zero;

            // Tilt
            if (TiltEnabled)
            {
                Vector3 forward = transform.forward;
                Vector3 right = transform.right;

                float forwardDot =
                    Vector3.Dot(velocity, forward);

                float forwardTilt = Mathf.Clamp(
                    forwardDot * Mathf.Deg2Rad * Pitch,
                    -Mathf.Deg2Rad * MaxPitch,
                    Mathf.Deg2Rad * MaxPitch
                );

                angles.x += forwardTilt;

                float rightDot =
                    Vector3.Dot(velocity, right);

                float sideTilt = Mathf.Clamp(
                    rightDot * Mathf.Deg2Rad * Roll,
                    -Mathf.Deg2Rad * MaxRoll,
                    Mathf.Deg2Rad * MaxRoll
                );

                angles.z -= sideTilt;
            }

            // Fall kick
            if (FallKickEnabled)
            {
                float fallRatio =
                    Mathf.Max(0f, _fallTimer / FallTime);

                float fallKickAmount =
                    fallRatio * _fallValue;

                angles.x -= fallKickAmount;
                offset.y -= fallKickAmount;
            }

            // Headbob
            if (HeadbobEnabled)
            {
                float pitchDelta =
                    headbobSin *
                    Mathf.Deg2Rad *
                    HeadbobPitch *
                    speed;

                angles.x -= pitchDelta;

                float rollDelta =
                    headbobSin *
                    Mathf.Deg2Rad *
                    HeadbobRoll *
                    speed;

                angles.z -= rollDelta;

                float headbobHeight =
                    headbobSin *
                    speed *
                    HeadbobUp;

                offset.y += headbobHeight;
            }

            transform.localPosition =
                _initialLocalPosition + offset;

            transform.localRotation =
                Quaternion.Euler(angles * Mathf.Rad2Deg);
        }

        public void AddFallKick(float fallStrength)
        {
            if (fallStrength < FallVelocityThreshold)
                return;

            _fallValue =
                Mathf.Deg2Rad * (fallStrength * 0.3f);

            _fallTimer = FallTime;
        }
    }
}
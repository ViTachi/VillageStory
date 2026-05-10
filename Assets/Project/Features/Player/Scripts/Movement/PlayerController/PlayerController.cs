using DG.Tweening;
using KinematicCharacterController;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public enum CharacterState
    {
        Default,
    }

    public enum BonusOrientationMethod
    {
        None,
        TowardsGravity,
        TowardsGroundSlopeAndGravity,
    }

    public class PlayerController : MonoBehaviour, ICharacterController, IPlayerController
    {
        [SerializeField] private KinematicCharacterMotor _motor;

        [Header("Stable Movement")]
        [SerializeField] private float _maxStableMoveSpeed = 10f;
        [SerializeField] private float _stableMovementSharpness = 15f;
        [SerializeField] private float _orientationSharpness = 10f;

        [Header("Air Movement")]
        [SerializeField] private float _maxAirMoveSpeed = 15f;
        [SerializeField] private float _airAccelerationSpeed = 15f;
        [SerializeField] private float _drag = 0.1f;

        [Header("Jumping")]
        [SerializeField] private bool _allowJumpingWhenSliding = true;
        [SerializeField] private float _jumpUpSpeed = 10f;
        [SerializeField] private float _jumpScalableForwardSpeed = 10f;
        [SerializeField] private float _jumpPreGroundingGraceTime = 0f;
        [SerializeField] private float _jumpPostGroundingGraceTime = 0f;

        [Header("Misc")]
        [SerializeField] private List<Collider> _ignoredColliders = new List<Collider>();
        [SerializeField] private BonusOrientationMethod _bonusOrientationMethod = BonusOrientationMethod.None;
        [SerializeField] private float _bonusOrientationSharpness = 10f;
        [SerializeField] private Vector3 _gravity = new Vector3(0, -30f, 0);
        [SerializeField] private Transform _moveDirection;
        [SerializeField] private Transform _headAnchor;
        [SerializeField] private float _defaultCapsuleHeight = 2f;
        [SerializeField] private float _crouchedCapsuleHeight = 1f;
        [SerializeField] private float _capsuleRadius = 0.5f;

        private Collider[] _probedColliders = new Collider[8];
        private Vector3 _moveVector;
        private Vector3 _moveInput;
        private Vector3 _lookInputVector;
        private bool _jumpRequested = false;
        private bool _jumpConsumed = false;
        private bool _jumpedThisFrame = false;
        private float _timeSinceJumpRequested = Mathf.Infinity;
        private float _timeSinceLastAbleToJump = 0f;
        private Vector3 _internalVelocityAdd = Vector3.zero;
        private bool _shouldBeCrouching = false;
        private bool _isCrouching = false;
        private Tweener _crouchHeadTweener = null;
        private float _currentSpeed = 0;

        public event Action<float> OnLandedEvent;
        public CharacterState CurrentCharacterState { get; private set; }
        public float CurrentSpeed => _currentSpeed;
        public Vector3 MoveVector => _moveVector;
        public Vector3 Position => transform.position;
        public bool IsGrounded => _motor.GroundingStatus.IsStableOnGround;
        public Vector3 Velocity => _motor.Velocity;

        private void Awake()
        {
            // Handle initial state
            TransitionToState(CharacterState.Default);

            // Assign the characterController to the motor
            _motor.CharacterController = this;
            _motor.SetCapsuleDimensions(_capsuleRadius, _defaultCapsuleHeight, _defaultCapsuleHeight * 0.5f);
        }

        private void OnDisable()
        {
            DOTween.Kill(this);
        }

        /// <summary>
        /// Handles movement state transitions and enter/exit callbacks
        /// </summary>
        public void TransitionToState(CharacterState newState)
        {
            CharacterState tmpInitialState = CurrentCharacterState;
            OnStateExit(tmpInitialState, newState);
            CurrentCharacterState = newState;
            OnStateEnter(newState, tmpInitialState);
        }

        /// <summary>
        /// Event when entering a state
        /// </summary>
        public void OnStateEnter(CharacterState state, CharacterState fromState)
        {
            switch (state)
            {
                case CharacterState.Default:
                    {
                        break;
                    }
            }
        }

        /// <summary>
        /// Event when exiting a state
        /// </summary>
        public void OnStateExit(CharacterState state, CharacterState toState)
        {
            switch (state)
            {
                case CharacterState.Default:
                    {
                        break;
                    }
            }
        }

        public void SetMoveSpeed(float moveSpeed)
        {
            _maxStableMoveSpeed = moveSpeed;
            _maxAirMoveSpeed = moveSpeed;
        }

        public void SetJumpUpSpeed(float jumpUpSpeed)
        {
            _jumpUpSpeed = jumpUpSpeed;
        }

        public void Move(Vector3 moveVector)
        {
            _moveInput = Vector3.ClampMagnitude(moveVector, 1f);
        }

        public void SetPosition(Vector3 position)
        {
            _motor.SetPosition(position);
        }

        public void Jump()
        {
            _timeSinceJumpRequested = 0f;
            _jumpRequested = true;
        }

        public void SetCrouch(bool crouch)
        {
            _shouldBeCrouching = crouch;
            if (crouch)
            {
                if (!_isCrouching)
                {
                    _isCrouching = true;
                    _motor.SetCapsuleDimensions(_capsuleRadius, _crouchedCapsuleHeight, _crouchedCapsuleHeight * 0.5f);
                    _crouchHeadTweener?.Kill();
                    _crouchHeadTweener = _headAnchor.DOScale(_crouchedCapsuleHeight / _defaultCapsuleHeight, 0.25f).SetId(this);
                }
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is called before the character begins its movement update
        /// </summary>
        public void BeforeCharacterUpdate(float deltaTime)
        {
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is where you tell your character what its rotation should be right now. 
        /// This is the ONLY place where you should set the character's rotation
        /// </summary>
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                    {
                        if (_lookInputVector.sqrMagnitude > 0f && _orientationSharpness > 0f)
                        {
                            // Smoothly interpolate from current to target look direction
                            Vector3 smoothedLookInputDirection = Vector3.Slerp(_motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-_orientationSharpness * deltaTime)).normalized;

                            // Set the current rotation (which will be used by the KinematicCharacterMotor)
                            currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, _motor.CharacterUp);
                        }

                        Vector3 currentUp = (currentRotation * Vector3.up);
                        if (_bonusOrientationMethod == BonusOrientationMethod.TowardsGravity)
                        {
                            // Rotate from current up to invert gravity
                            Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, -_gravity.normalized, 1 - Mathf.Exp(-_bonusOrientationSharpness * deltaTime));
                            currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                        }
                        else if (_bonusOrientationMethod == BonusOrientationMethod.TowardsGroundSlopeAndGravity)
                        {
                            if (_motor.GroundingStatus.IsStableOnGround)
                            {
                                Vector3 initialCharacterBottomHemiCenter = _motor.TransientPosition + (currentUp * _motor.Capsule.radius);

                                Vector3 smoothedGroundNormal = Vector3.Slerp(_motor.CharacterUp, _motor.GroundingStatus.GroundNormal, 1 - Mathf.Exp(-_bonusOrientationSharpness * deltaTime));
                                currentRotation = Quaternion.FromToRotation(currentUp, smoothedGroundNormal) * currentRotation;

                                // Move the position to create a rotation around the bottom hemi center instead of around the pivot
                                _motor.SetTransientPosition(initialCharacterBottomHemiCenter + (currentRotation * Vector3.down * _motor.Capsule.radius));
                            }
                            else
                            {
                                Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, -_gravity.normalized, 1 - Mathf.Exp(-_bonusOrientationSharpness * deltaTime));
                                currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                            }
                        }
                        else
                        {
                            Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, Vector3.up, 1 - Mathf.Exp(-_bonusOrientationSharpness * deltaTime));
                            currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is where you tell your character what its velocity should be right now. 
        /// This is the ONLY place where you can set the character's velocity
        /// </summary>
        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(_moveDirection.rotation * Vector3.forward, _motor.CharacterUp).normalized;
            Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, _motor.CharacterUp);
            _moveVector = cameraPlanarRotation * _moveInput;

            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                    {
                        // Ground movement
                        if (_motor.GroundingStatus.IsStableOnGround)
                        {
                            float currentVelocityMagnitude = currentVelocity.magnitude;

                            Vector3 effectiveGroundNormal = _motor.GroundingStatus.GroundNormal;

                            // Reorient velocity on slope
                            currentVelocity = _motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;

                            // Calculate target velocity
                            Vector3 inputRight = Vector3.Cross(_moveVector, _motor.CharacterUp);
                            Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * _moveVector.magnitude;
                            Vector3 targetMovementVelocity = reorientedInput * _maxStableMoveSpeed;

                            // Smooth movement Velocity
                            currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-_stableMovementSharpness * deltaTime));
                        }
                        // Air movement
                        else
                        {
                            // Add move input
                            if (_moveVector.sqrMagnitude > 0f)
                            {
                                Vector3 addedVelocity = _moveVector * _airAccelerationSpeed * deltaTime;

                                Vector3 currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, _motor.CharacterUp);

                                // Limit air velocity from inputs
                                if (currentVelocityOnInputsPlane.magnitude < _maxAirMoveSpeed)
                                {
                                    // clamp addedVel to make total vel not exceed max vel on inputs plane
                                    Vector3 newTotal = Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity, _maxAirMoveSpeed);
                                    addedVelocity = newTotal - currentVelocityOnInputsPlane;
                                }
                                else
                                {
                                    // Make sure added vel doesn't go in the direction of the already-exceeding velocity
                                    if (Vector3.Dot(currentVelocityOnInputsPlane, addedVelocity) > 0f)
                                    {
                                        addedVelocity = Vector3.ProjectOnPlane(addedVelocity, currentVelocityOnInputsPlane.normalized);
                                    }
                                }

                                // Prevent air-climbing sloped walls
                                if (_motor.GroundingStatus.FoundAnyGround)
                                {
                                    if (Vector3.Dot(currentVelocity + addedVelocity, addedVelocity) > 0f)
                                    {
                                        Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(_motor.CharacterUp, _motor.GroundingStatus.GroundNormal), _motor.CharacterUp).normalized;
                                        addedVelocity = Vector3.ProjectOnPlane(addedVelocity, perpenticularObstructionNormal);
                                    }
                                }

                                // Apply added velocity
                                currentVelocity += addedVelocity;
                            }

                            // Gravity
                            currentVelocity += _gravity * deltaTime;

                            // Drag
                            currentVelocity *= (1f / (1f + (_drag * deltaTime)));
                        }

                        // Handle jumping
                        _jumpedThisFrame = false;
                        _timeSinceJumpRequested += deltaTime;
                        if (_jumpRequested)
                        {
                            // See if we actually are allowed to jump
                            if (!_jumpConsumed && ((_allowJumpingWhenSliding ? _motor.GroundingStatus.FoundAnyGround : _motor.GroundingStatus.IsStableOnGround) || _timeSinceLastAbleToJump <= _jumpPostGroundingGraceTime))
                            {
                                // Calculate jump direction before ungrounding
                                Vector3 jumpDirection = _motor.CharacterUp;
                                if (_motor.GroundingStatus.FoundAnyGround && !_motor.GroundingStatus.IsStableOnGround)
                                {
                                    jumpDirection = _motor.GroundingStatus.GroundNormal;
                                }

                                // Makes the character skip ground probing/snapping on its next update. 
                                // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
                                _motor.ForceUnground();

                                // Add to the return velocity and reset jump state
                                currentVelocity += (jumpDirection * _jumpUpSpeed) - Vector3.Project(currentVelocity, _motor.CharacterUp);
                                currentVelocity += (_moveVector * _jumpScalableForwardSpeed);
                                _jumpRequested = false;
                                _jumpConsumed = true;
                                _jumpedThisFrame = true;
                            }
                        }

                        // Take into account additive velocity
                        if (_internalVelocityAdd.sqrMagnitude > 0f)
                        {
                            currentVelocity += _internalVelocityAdd;
                            _internalVelocityAdd = Vector3.zero;
                        }

                        _currentSpeed = currentVelocity.magnitude;

                        break;
                    }
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is called after the character has finished its movement update
        /// </summary>
        public void AfterCharacterUpdate(float deltaTime)
        {
            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                    {
                        // Handle jump-related values
                        {
                            // Handle jumping pre-ground grace period
                            if (_jumpRequested && _timeSinceJumpRequested > _jumpPreGroundingGraceTime)
                            {
                                _jumpRequested = false;
                            }

                            if (_allowJumpingWhenSliding ? _motor.GroundingStatus.FoundAnyGround : _motor.GroundingStatus.IsStableOnGround)
                            {
                                // If we're on a ground surface, reset jumping values
                                if (!_jumpedThisFrame)
                                {
                                    _jumpConsumed = false;
                                }
                                _timeSinceLastAbleToJump = 0f;
                            }
                            else
                            {
                                // Keep track of time since we were last able to jump (for grace period)
                                _timeSinceLastAbleToJump += deltaTime;
                            }
                        }

                        // Handle uncrouching
                        if (_isCrouching && !_shouldBeCrouching)
                        {
                            // Do an overlap test with the character's standing height to see if there are any obstructions
                            _motor.SetCapsuleDimensions(_capsuleRadius, _defaultCapsuleHeight, _defaultCapsuleHeight * 0.5f);
                            if (_motor.CharacterOverlap(
                                _motor.TransientPosition,
                                _motor.TransientRotation,
                                _probedColliders,
                                _motor.CollidableLayers,
                                QueryTriggerInteraction.Ignore) > 0)
                            {
                                // If obstructions, just stick to crouching dimensions
                                _motor.SetCapsuleDimensions(_capsuleRadius, _crouchedCapsuleHeight, _crouchedCapsuleHeight * 0.5f);
                            }
                            else
                            {
                                // If no obstructions, uncrouch
                                _crouchHeadTweener?.Kill();
                                _crouchHeadTweener = _headAnchor.DOScale(1f, 0.25f).SetId(this);
                                _isCrouching = false;
                            }
                        }
                        break;
                    }
            }
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            // Handle landing and leaving ground
            if (_motor.GroundingStatus.IsStableOnGround && !_motor.LastGroundingStatus.IsStableOnGround)
            {
                OnLanded();
            }
            else if (!_motor.GroundingStatus.IsStableOnGround && _motor.LastGroundingStatus.IsStableOnGround)
            {
                OnLeaveStableGround();
            }
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            if (_ignoredColliders.Count == 0)
            {
                return true;
            }

            if (_ignoredColliders.Contains(coll))
            {
                return false;
            }

            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void AddVelocity(Vector3 velocity)
        {
            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                    {
                        _internalVelocityAdd += velocity;
                        break;
                    }
            }
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
        }

        protected void OnLanded()
        {
            OnLandedEvent?.Invoke(Mathf.Abs(_motor.PreviousVelocity.y));
        }

        protected void OnLeaveStableGround()
        {
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
        }
    }
}
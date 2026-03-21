using KinematicCharacterController;
using UnityEngine;

namespace Gameplay
{
    public interface IPlayerController
    {
        CharacterState CurrentCharacterState { get; }
        float CurrentSpeed { get; }
        Vector3 MoveVector { get; }
        Vector3 Position { get; }

        void AddVelocity(Vector3 velocity);
        void AfterCharacterUpdate(float deltaTime);
        void BeforeCharacterUpdate(float deltaTime);
        bool IsColliderValidForCollisions(Collider coll);
        void Jump();
        void Move(Vector3 moveVector);
        void OnDiscreteCollisionDetected(Collider hitCollider);
        void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport);
        void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport);
        void OnStateEnter(CharacterState state, CharacterState fromState);
        void OnStateExit(CharacterState state, CharacterState toState);
        void PostGroundingUpdate(float deltaTime);
        void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport);
        void SetCrouch(bool crouch);
        void SetPosition(Vector3 position);
        void TransitionToState(CharacterState newState);
        void UpdateRotation(ref Quaternion currentRotation, float deltaTime);
        void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime);
    }
}
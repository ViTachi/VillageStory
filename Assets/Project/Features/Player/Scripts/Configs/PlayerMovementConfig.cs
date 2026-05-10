using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerMovementConfig", menuName = "Configs/PlayerMovementConfig")]
    public class PlayerMovementConfig : ScriptableObject
    {
        [field: SerializeField] public float MoveSpeed = 5f;
        [field: SerializeField] public float JumpUpSpeed = 5f;
        [field: SerializeField] public float Threshold = 0.01f;
        [field: SerializeField] public float Sensitivity = 1f;
        [field: SerializeField] public float TopClamp = 80.0f;
        [field: SerializeField] public float BottomClamp = -80.0f;
    }
}
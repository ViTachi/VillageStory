using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerMovementConfig", menuName = "Configs/PlayerMovementConfig")]
    public class PlayerMovementConfig : ScriptableObject
    {
        [field: SerializeField] public float MoveSpeed { get; private set; } = 5f;
        [field: SerializeField] public float JumpUpSpeed { get; private set; } = 5f;
        [field: SerializeField] public float Threshold { get; private set; } = 0.01f;
        [field: SerializeField] public float TopClamp { get; private set; } = 80.0f;
        [field: SerializeField] public float BottomClamp { get; private set; } = -80.0f;
    }
}
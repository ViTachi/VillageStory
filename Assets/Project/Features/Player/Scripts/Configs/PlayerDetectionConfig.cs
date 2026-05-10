using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerDetectionConfig", menuName = "Configs/PlayerDetectionConfig")]
    public class PlayerDetectionConfig : ScriptableObject
    {
        [field: SerializeField] public float DetectDistance { get; private set; } = 2f;
    }
}
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "NPCData", menuName = "Dialogue/NPC Data")]
    public class NPCData : ScriptableObject
    {
        [field: SerializeField] public string DisplayName { get; private set; }
        [field: SerializeField] public Color NameColor { get; private set; } = Color.white;
    }
}

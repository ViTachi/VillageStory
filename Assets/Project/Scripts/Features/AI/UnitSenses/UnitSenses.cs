using Sirenix.OdinInspector;
using UnityEngine;

namespace AI
{
    public class UnitSenses : MonoBehaviour
    {
        [LabelText("Move", SdfIconType.ArrowsMove), LabelWidth(85)]
        [GUIColor(1f, 0.8f, 0.6f, 1f)]
        [SerializeField] private Movement _movement;

        [LabelText("Vision", SdfIconType.EyeFill), LabelWidth(85)]
        [GUIColor(0.7f, 0.88f, 1f, 1f)]
        [SerializeField] private Vision _vision;

        [LabelText("Hear", SdfIconType.Headphones), LabelWidth(85)]
        [GUIColor(1f, 0.95f, 0.65f, 1f)]
        [SerializeField] private Hearing _hearing;

        public Movement Movement => _movement;
        public Vision Vision => _vision;
        public Hearing Hearing => _hearing;
    }
}
using UnityEngine;

namespace AI
{
    public class UnitSenses : MonoBehaviour
    {
        [SerializeField] private Movement _movement;
        [SerializeField] private Vision _vision;
        [SerializeField] private Hearing _hearing;

        public Movement Movement => _movement;
        public Vision Vision => _vision;
        public Hearing Hearing => _hearing;
    }
}
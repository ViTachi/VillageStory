using UnityEngine;

namespace Gameplay.Items
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private HingeJoint _hingeJoint;
        [SerializeField] private Rigidbody _rb;

        private bool _isOpened = false;

        public bool IsOpened => _isOpened;

        public void Open(float offset)
        {
            _isOpened = true;
            SetJointSpringPosition(offset);
        }

        public void Close()
        {
            _isOpened = false;
            SetJointSpringPosition(0);
        }

        private void SetJointSpringPosition(float position)
        {
            _hingeJoint.spring = new JointSpring()
            {
                spring = _hingeJoint.spring.spring,
                damper = _hingeJoint.spring.damper,
                targetPosition = position
            };
        }
    }
}
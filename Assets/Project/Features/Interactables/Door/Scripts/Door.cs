using UnityEngine;

namespace Interactables
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private new HingeJoint hingeJoint;
        [SerializeField] private Rigidbody rb;

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
            hingeJoint.spring = new JointSpring()
            {
                spring = hingeJoint.spring.spring,
                damper = hingeJoint.spring.damper,
                targetPosition = position
            };
        }
    }
}
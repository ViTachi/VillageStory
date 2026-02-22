using UnityEngine;

public class AutoCrouch : MonoBehaviour
{
    [SerializeField] private float speedTrashhold = 0.5f;
    [SerializeField] private float directionLimit = 0.5f;
    [SerializeField] private PlayerController playerController;

    private void Awake()
    {
#if UNITY_EDITOR
        enabled = true;
#elif UNITY_ANDROID || UNITY_IOS
        enabled = true;
#else
        enabled = false;
#endif
    }

    private void OnTriggerStay(Collider other)
    {
        if (!enabled) return;

        CrouchArea crouchArea = other.GetComponent<CrouchArea>();
        if (crouchArea != null)
        {
            if (Vector3.Dot(playerController.MoveVector, (crouchArea.CenterPosition - playerController.Position).normalized) > directionLimit
                && playerController.CurrentSpeed > speedTrashhold)
            {
                playerController.SetCrouch(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!enabled) return;

        CrouchArea crouchArea = other.GetComponent<CrouchArea>();
        if (crouchArea != null)
        {
            playerController.SetCrouch(false);
        }
    }
}

using UnityEngine;

namespace Player
{
    /// <summary>
    /// Default attachable on NPC colliders; override <see cref="BeginDialogue"/> or replace with your own <see cref="IDialoguePartner"/>.
    /// </summary>
    public class NpcDialoguePartner : MonoBehaviour, IDialoguePartner
    {
        public virtual void BeginDialogue()
        {
            // Hook dialogue system / UI here.
        }

        public virtual void EndDialogue()
        {
            // Tear down dialogue UI / logic here.
        }
    }
}

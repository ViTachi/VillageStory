using UnityEngine;

namespace Player
{
    /// <summary>
    /// Default attachable on NPC colliders; override <see cref="BeginDialogue"/> or replace with your own <see cref="IDialoguePartner"/>.
    /// </summary>
    public class NpcDialoguePartner : MonoBehaviour, IDialoguePartner
    {
        [SerializeField] private DialogueData _dialogueData;

        public DialogueData GetDialogueData() => _dialogueData;

        public virtual void BeginDialogue()
        {
        }

        public virtual void EndDialogue()
        {
        }
    }
}

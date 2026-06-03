using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Player
{
    public class NpcDialoguePartner : MonoBehaviour, IDialoguePartner
    {
        [SerializeField] private List<NpcDialogueEntry> _dialogueEntries = new();

        [Inject] private ISaveLoadService _saveLoadService;

        public DialogueData GetDialogueData()
        {
            foreach (NpcDialogueEntry entry in _dialogueEntries)
            {
                if (entry.IsAvailable(_saveLoadService.Data))
                    return entry.Dialogue;
            }

            return null;
        }

        public virtual void BeginDialogue()
        {
        }

        public virtual void EndDialogue()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class NpcDialogueEntry
    {
        [SerializeField] private DialogueData _dialogue;

        [SerializeField, SerializeReference, SubclassSelector]
        private List<IGameCondition> _conditions = new();

        public DialogueData Dialogue => _dialogue;

        public bool IsAvailable(SaveData saveData)
        {
            if (_dialogue == null)
                return false;

            foreach (IGameCondition condition in _conditions)
            {
                if (condition == null)
                    continue;

                if (!condition.IsMet(saveData))
                    return false;
            }

            return true;
        }
    }
}

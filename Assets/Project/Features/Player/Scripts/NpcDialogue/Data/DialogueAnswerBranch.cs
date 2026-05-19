using System;
using System.Collections.Generic;
using MackySoft.SerializeReferenceExtensions;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class DialogueAnswerBranch
    {
        [field: SerializeField] public string Text { get; private set; }

        [field: SerializeReference, SubclassSelector]
        public List<DialogueAction> Actions { get; private set; } = new();
    }
}

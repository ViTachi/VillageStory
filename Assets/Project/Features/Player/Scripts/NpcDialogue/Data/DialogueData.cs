using System;
using System.Collections.Generic;
using UnityEngine;
using MackySoft.SerializeReferenceExtensions;

namespace Player
{

    [Serializable]
    public abstract class DialogueAction
    {

    }

    [Serializable]
    public class DialogueAnswer : DialogueAction
    {
        [field: SerializeField] public List<string> Variants { get; private set; }
    }

    [Serializable]
    public class DialogueReplica : DialogueAction
    {
        [field: SerializeField] public NPCData NpcData { get; private set; }
        [field: SerializeField, TextArea(3, 10)] public string Text { get; private set; }
    }

    [CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue/Dialogue Data")]
    public class DialogueData : ScriptableObject
    {
        [field: SerializeReference, SubclassSelector]
        public List<DialogueAction> Actions { get; private set; }
    }
}

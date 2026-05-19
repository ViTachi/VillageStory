using System.Collections.Generic;
using MackySoft.SerializeReferenceExtensions;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue/Dialogue Data")]
    public class DialogueData : ScriptableObject
    {
        [field: SerializeReference, SubclassSelector]
        public List<DialogueAction> Actions { get; private set; }
    }
}

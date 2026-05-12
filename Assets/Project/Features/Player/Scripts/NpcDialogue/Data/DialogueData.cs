using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class DialogueReplica
    {
        [SerializeField] private NPCData _npcData;
        [SerializeField, TextArea(3, 10)] private string _text;

        public NPCData NpcData => _npcData;
        public string Text => _text;
    }

    [CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue/Dialogue Data")]
    public class DialogueData : ScriptableObject
    {
        [SerializeField] private List<DialogueReplica> _replicas = new List<DialogueReplica>();

        public IReadOnlyList<DialogueReplica> Replicas => _replicas;
    }
}

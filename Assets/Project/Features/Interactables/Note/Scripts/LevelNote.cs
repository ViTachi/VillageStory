using Player;
using UnityEngine;

namespace Interactables
{
    public class LevelNote : MonoBehaviour, ILevelNote
    {
        [SerializeField][TextArea] private string _text;
        [SerializeField] private TMPro.TMP_Text _label;

        public string Text => _text;

        private void Awake()
        {
            _label.text = _text;
        }
    }
}


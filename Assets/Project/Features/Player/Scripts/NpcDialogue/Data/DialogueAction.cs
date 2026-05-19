using System;
using Cysharp.Threading.Tasks;

namespace Player
{
    [Serializable]
    public abstract class DialogueAction
    {
        public abstract UniTask Execute(DialogueContext context);
    }
}

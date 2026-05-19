using System.Threading;

namespace Player
{
    public class DialogueContext
    {
        public DialogueContext(CancellationToken cancellationToken)
        {
            CancellationToken = cancellationToken;
        }

        public CancellationToken CancellationToken { get; }
    }
}

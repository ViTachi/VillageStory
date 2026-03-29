using System.Threading;
using Cysharp.Threading.Tasks;

namespace Player
{
    public class CleanupPlayerState : State
    {
        public override UniTask Enter(CancellationToken token)
        {
            return base.Enter(token);
        }
    }
}
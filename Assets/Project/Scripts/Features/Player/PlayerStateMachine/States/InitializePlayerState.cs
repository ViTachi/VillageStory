using System.Threading;
using Cysharp.Threading.Tasks;

namespace Player
{
    public class InitializePlayerState : State
    {
        public override UniTask Enter(CancellationToken token)
        {
            _stateMachine.EnterState<MoveState>();
            return base.Enter(token);
        }
    }
}
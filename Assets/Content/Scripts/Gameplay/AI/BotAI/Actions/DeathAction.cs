using System.Threading;
using Cysharp.Threading.Tasks;

namespace Gameplay.AI
{
    public class DeathAction : UnitAction<object>
    {
        protected async override UniTask RunAction(CancellationToken token)
        {
            // death animation
            await UniTask.Delay(500, cancellationToken: token);
        }

        public override void StopAction()
        {
            base.StopAction();
            //stop animation
        }
    }
}
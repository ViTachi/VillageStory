using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Player
{
    public class InitializePlayerState : State
    {
        [Inject] private CollectingItemsViewController _collectingItemsViewController;
        [Inject] private InteractionViewController _interactionViewController;
        [Inject] private LevelNotesViewController _levelNotesViewController;

        public override UniTask Enter(CancellationToken token)
        {
            _collectingItemsViewController.Initialize();
            _interactionViewController.Initialize();
            _levelNotesViewController.Initialize();

            _stateMachine.EnterState<MoveState>();
            return base.Enter(token);
        }
    }
}
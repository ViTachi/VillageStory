using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Player
{
    public class CleanupPlayerState : State
    {
        [Inject] private CollectingItemsViewController _collectingItemsViewController;
        [Inject] private InteractionViewController _interactionViewController;
        [Inject] private LevelNotesViewController _levelNotesViewController;
        [Inject] private NpcDialogueViewController _npcDialogueViewController;

        public override UniTask Enter(CancellationToken token)
        {
            _collectingItemsViewController.Cleanup();
            _interactionViewController.Cleanup();
            _levelNotesViewController.Cleanup();
            _npcDialogueViewController.Cleanup();
            
            return base.Enter(token);
        }
    }
}
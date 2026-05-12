using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Player
{
    public class MoveState : State
    {
        [Inject] private IPlayerMovementService _playerMovementService;
        [Inject] private IPlayerInteractService _playerInteractService;
        [Inject] private ICollectingItemsService _collectingItemsService;
        [Inject] private ILevelNotesService _levelNotesService;
        [Inject] private INpcDialogueService _npcDialogueService;
        [Inject] private IPointerDetectionService _pointerDetectionService;

        public override UniTask Enter(CancellationToken token)
        {
            _playerMovementService.Initialize();
            _playerInteractService.Initialize();
            _collectingItemsService.Initialize();
            _levelNotesService.Initialize();
            _npcDialogueService.Initialize();
            _pointerDetectionService.Initialize();

            _levelNotesService.OnShowNote += OnShowNote;
            _npcDialogueService.OnShowDialogue += OnShowDialogue;

            return base.Enter(token);
        }

        public override UniTask Exit(CancellationToken token)
        {
            _playerMovementService.Cleanup();
            _playerInteractService.Cleanup();
            _collectingItemsService.Cleanup();
            _levelNotesService.Cleanup();
            _npcDialogueService.Cleanup();
            _pointerDetectionService.Cleanup();

            _levelNotesService.OnShowNote -= OnShowNote;
            _npcDialogueService.OnShowDialogue -= OnShowDialogue;

            return base.Exit(token);
        }

        private void OnShowNote()
        {
            _stateMachine.EnterState<ReadNoteState>();
        }

        private void OnShowDialogue()
        {
            _stateMachine.EnterState<NpcDialogueState>();
        }
    }
}
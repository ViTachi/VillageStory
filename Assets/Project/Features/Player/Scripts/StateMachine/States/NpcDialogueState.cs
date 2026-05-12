using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Player
{
    public class NpcDialogueState : State
    {
        [Inject] private INpcDialogueService _npcDialogueService;

        public override UniTask Enter(CancellationToken token)
        {
            _npcDialogueService.Initialize();
            _npcDialogueService.OnHideDialogue += OnHideDialogue;
            return base.Enter(token);
        }

        public override UniTask Exit(CancellationToken token)
        {
            _npcDialogueService.OnHideDialogue -= OnHideDialogue;
            _npcDialogueService.Cleanup();
            return base.Exit(token);
        }

        private void OnHideDialogue()
        {
            _stateMachine.EnterState<MoveState>();
        }
    }
}

using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Player
{
    public class ReadNoteState : State
    {
        [Inject] private ILevelNotesService _levelNotesService;

        public override UniTask Enter(CancellationToken token)
        {
            _levelNotesService.Initialize();
            _levelNotesService.OnHideNote += OnHideNote;
            return base.Enter(token);
        }

        public override UniTask Exit(CancellationToken token)
        {
            _levelNotesService.OnHideNote -= OnHideNote;
            _levelNotesService.Cleanup();
            return base.Exit(token);
        }

        private void OnHideNote()
        {
            _stateMachine.EnterState<MoveState>();
        }
    }
}
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Input;
using UI;
using UI.PlayerHud;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public class NpcDialogueService : INpcDialogueService
    {
        [Inject] private PlayerInput _playerInput;
        [Inject] private IPointerDetectionService _pointerDetectionService;
        [Inject] private IUIService _uIService;
        [Inject] private DiContainer _container;

        public event Action OnShowDialogue;
        public event Action OnHideDialogue;

        private bool _dialogueActive;
        private IDialoguePartner _currentPartner;
        private PlayerHud _playerHud;
        private CancellationTokenSource _dialogueCts;

        public void Initialize()
        {
            _playerHud = _uIService.GetScreen<PlayerHud, object, object>();
            _playerInput.Player.Interact.started += TryStartDialogue;
        }

        public void Cleanup()
        {
            _playerInput.Player.Interact.started -= TryStartDialogue;
            CancelDialogue();
        }

        private void TryStartDialogue(CallbackContext _)
        {
            if (_dialogueActive)
                return;

            if (_pointerDetectionService.ItemInView == null)
                return;

            if (_pointerDetectionService.ItemInView.TryGetComponent(out IDialoguePartner partner))
                ShowDialogue(partner);
        }

        private void ShowDialogue(IDialoguePartner partner)
        {
            _dialogueActive = true;
            _currentPartner = partner;
            partner.BeginDialogue();
            _pointerDetectionService.SetActive(false);
            OnShowDialogue?.Invoke();

            RunDialogueAsync(partner.GetDialogueData()).Forget();
        }

        private async UniTaskVoid RunDialogueAsync(DialogueData data)
        {
            _dialogueCts = new CancellationTokenSource();
            var context = new DialogueContext(_dialogueCts.Token);

            try
            {
                _playerHud.DialogueView.Show();

                if (data?.Actions != null)
                {
                    foreach (DialogueAction action in data.Actions)
                    {
                        _container.Inject(action);
                        await action.Execute(context);
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                EndDialogue();
            }
        }

        private void EndDialogue()
        {
            if (!_dialogueActive)
                return;

            _dialogueActive = false;
            _dialogueCts?.Cancel();
            _dialogueCts?.Dispose();
            _dialogueCts = null;

            _playerHud.DialogueView.Hide();
            _playerHud.DialogueAnswersView.Hide();

            _currentPartner?.EndDialogue();
            _currentPartner = null;
            _pointerDetectionService.SetActive(true);
            OnHideDialogue?.Invoke();
        }

        private void CancelDialogue()
        {
            if (!_dialogueActive)
                return;

            _dialogueCts?.Cancel();
        }
    }
}

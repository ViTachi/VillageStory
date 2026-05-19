using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Input;
using UI;
using UI.PlayerHud;
using UnityEngine;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    [Serializable]
    public class DialogueReplica : DialogueAction
    {
        [Inject] private IUIService _uiService;
        [Inject] private PlayerInput _playerInput;

        [field: SerializeField] public NPCData NpcData { get; private set; }
        [field: SerializeField, TextArea(3, 10)] public string Text { get; private set; }

        public override async UniTask Execute(DialogueContext context)
        {
            DialogueView dialogueView = _uiService.GetScreen<PlayerHud, object, object>().DialogueView;
            dialogueView.ShowLine(NpcData, Text);

            while (true)
            {
                await WaitForAdvanceInputAsync(context.CancellationToken);

                if (dialogueView.TryCompleteReveal())
                    break;
            }
        }

        private async UniTask WaitForAdvanceInputAsync(CancellationToken cancellationToken)
        {
            var completionSource = new UniTaskCompletionSource();

            void OnAdvance(CallbackContext _) => completionSource.TrySetResult();

            _playerInput.Player.Interact.started += OnAdvance;
            _playerInput.Player.Use.started += OnAdvance;
            _playerInput.Player.Jump.started += OnAdvance;
            _playerInput.Player.Submit.started += OnAdvance;

            try
            {
                await completionSource.Task.AttachExternalCancellation(cancellationToken);
            }
            finally
            {
                _playerInput.Player.Interact.started -= OnAdvance;
                _playerInput.Player.Use.started -= OnAdvance;
                _playerInput.Player.Jump.started -= OnAdvance;
                _playerInput.Player.Submit.started -= OnAdvance;
            }
        }
    }
}

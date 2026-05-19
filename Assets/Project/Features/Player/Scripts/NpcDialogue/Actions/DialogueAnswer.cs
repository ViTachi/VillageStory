using System;
using System.Collections.Generic;
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
    public class DialogueAnswer : DialogueAction
    {
        [Inject] private IUIService _uiService;
        [Inject] private PlayerInput _playerInput;
        [Inject] private DiContainer _container;

        [field: SerializeField] public List<DialogueAnswerBranch> Branches { get; private set; } = new();

        public override async UniTask Execute(DialogueContext context)
        {
            if (Branches == null || Branches.Count == 0)
                return;

            PlayerHud playerHud = _uiService.GetScreen<PlayerHud, object, object>();
            DialogueView dialogueView = playerHud.DialogueView;
            DialogueAnswersView answersView = playerHud.DialogueAnswersView;
            IReadOnlyList<string> labels = CollectBranchLabels();

            dialogueView.Hide();

            int selectedIndex = await WaitForAnswerSelectionAsync(answersView, labels, context.CancellationToken);
            answersView.Hide();

            if (selectedIndex < 0 || selectedIndex >= Branches.Count)
            {
                dialogueView.Show();
                return;
            }

            dialogueView.Show();
            await RunBranchActionsAsync(Branches[selectedIndex].Actions, context);
        }

        private async UniTask<int> WaitForAnswerSelectionAsync(
            DialogueAnswersView answersView,
            IReadOnlyList<string> labels,
            CancellationToken cancellationToken)
        {
            var selectionSource = new UniTaskCompletionSource<int>();

            void OnSelected(int index) => selectionSource.TrySetResult(index);

            void OnConfirmAnswer(CallbackContext _) => answersView.TrySubmitFocusedAnswer();

            answersView.ShowAnswers(labels, OnSelected);
            PlayerCursor.SetUnlockedForUi();

            _playerInput.Player.Interact.started += OnConfirmAnswer;
            _playerInput.Player.Jump.started += OnConfirmAnswer;

            try
            {
                return await selectionSource.Task.AttachExternalCancellation(cancellationToken);
            }
            finally
            {
                _playerInput.Player.Interact.started -= OnConfirmAnswer;
                _playerInput.Player.Jump.started -= OnConfirmAnswer;
                PlayerCursor.SetLockedForGameplay();
            }
        }

        private async UniTask RunBranchActionsAsync(IReadOnlyList<DialogueAction> actions, DialogueContext context)
        {
            if (actions == null)
                return;

            foreach (DialogueAction action in actions)
            {
                _container.Inject(action);
                await action.Execute(context);
            }
        }

        private IReadOnlyList<string> CollectBranchLabels()
        {
            var labels = new string[Branches.Count];
            for (int i = 0; i < Branches.Count; i++)
                labels[i] = Branches[i]?.Text ?? string.Empty;
            return labels;
        }
    }
}

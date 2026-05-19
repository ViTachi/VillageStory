using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace UI.PlayerHud
{
    public class PlayerHud : Screen<object, object>
    {
        [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }
        [field: SerializeField] public KeyHintInfoView KeyHintInfoView { get; private set; }
        [field: SerializeField] public NoteView NoteView { get; private set; }
        [field: SerializeField] public DialogueView DialogueView { get; private set; }
        [field: SerializeField] public DialogueAnswersView DialogueAnswersView { get; private set; }

        public override async UniTask Show(object parameters, bool immediately,
            CancellationToken ct, Action<object> callback = null)
        {
            await base.Show(parameters, immediately, ct, callback);
            KeyHintInfoView.Initialize();
            CanvasGroup.alpha = 0f;
            await CanvasGroup.DOFade(1f, 0.2f)
                .SetLink(gameObject).ToUniTask();
        }

        public override async UniTask Hide(bool immediately, CancellationToken ct)
        {
            await base.Hide(immediately, ct);
            await CanvasGroup.DOFade(0f, 0.2f)
                .SetLink(gameObject).ToUniTask();
        }
    }
}
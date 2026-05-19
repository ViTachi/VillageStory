using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Player;
using TMPro;
using UnityEngine;

namespace UI.PlayerHud
{
    public class DialogueView : MonoBehaviour
    {
        /// <summary>Delay between showing each character (constant typewriter speed).</summary>
        private const float SecondsPerCharacter = 0.04f;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _speakerNameText;
        [SerializeField] private TMP_Text _lineText;

        private string _fullLineText = string.Empty;
        private bool _lineRevealComplete = true;
        private CancellationTokenSource _revealCts;

        public void Show()
        {
            CancelReveal();

            DOTween.Kill(this);
            _canvasGroup.alpha = 0f;
            transform.localScale = Vector3.one * 0.6f;
            transform.DOScale(1f, 0.2f).SetId(this).SetLink(gameObject);
            _canvasGroup.DOFade(1f, 0.15f).SetId(this).SetLink(gameObject);
        }

        public void Hide()
        {
            CancelReveal();

            DOTween.Kill(this);
            transform.DOScale(0.6f, 0.12f).SetId(this).SetLink(gameObject);
            _canvasGroup.DOFade(0f, 0.12f).SetId(this).SetLink(gameObject);
        }

        public void ShowLine(NPCData npc, string text)
        {
            CancelReveal();

            if (_speakerNameText != null)
            {
                if (npc != null)
                {
                    _speakerNameText.text = npc.DisplayName;
                    _speakerNameText.color = npc.NameColor;
                }
                else
                {
                    _speakerNameText.text = string.Empty;
                }
            }

            _fullLineText = text ?? string.Empty;
            if (_lineText != null)
                _lineText.text = string.Empty;

            if (string.IsNullOrEmpty(_fullLineText))
            {
                _lineRevealComplete = true;
                return;
            }

            _lineRevealComplete = false;
            _revealCts = new CancellationTokenSource();
            RevealLineAsync(_revealCts.Token).Forget();
        }

        /// <summary>
        /// If the line is still revealing: completes it instantly and returns false.
        /// If the line is fully shown: returns true.
        /// </summary>
        public bool TryCompleteReveal()
        {
            if (!_lineRevealComplete)
            {
                CompleteCurrentLineInstantly();
                return false;
            }

            return true;
        }

        private void OnDestroy()
        {
            CancelReveal();
        }

        private void CompleteCurrentLineInstantly()
        {
            CancelReveal();
            if (_lineText != null)
                _lineText.text = _fullLineText ?? string.Empty;
            _lineRevealComplete = true;
        }

        private void CancelReveal()
        {
            if (_revealCts == null) return;
            _revealCts.Cancel();
            _revealCts.Dispose();
            _revealCts = null;
        }

        private async UniTaskVoid RevealLineAsync(CancellationToken ct)
        {
            try
            {
                if (_lineText == null)
                {
                    _lineRevealComplete = true;
                    return;
                }

                for (int len = 1; len <= _fullLineText.Length; len++)
                {
                    ct.ThrowIfCancellationRequested();
                    _lineText.text = _fullLineText.Substring(0, len);
                    if (len < _fullLineText.Length)
                        await UniTask.Delay(TimeSpan.FromSeconds(SecondsPerCharacter), cancellationToken: ct,
                            delayTiming: PlayerLoopTiming.Update);
                }

                _lineRevealComplete = true;
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}

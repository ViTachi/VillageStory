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

        private DialogueData _data;
        private int _lineIndex;
        private string _fullLineText = string.Empty;
        private bool _lineRevealComplete = true;
        private CancellationTokenSource _revealCts;

        public void ShowDialogue(DialogueData data)
        {
            CancelReveal();

            _data = data;
            _lineIndex = 0;

            DOTween.Kill(this);
            _canvasGroup.alpha = 0f;
            transform.localScale = Vector3.one * 0.6f;
            transform.DOScale(1f, 0.2f).SetId(this).SetLink(gameObject);
            _canvasGroup.DOFade(1f, 0.15f).SetId(this).SetLink(gameObject);

            ApplyCurrentLine();
        }

        public void Hide()
        {
            CancelReveal();

            DOTween.Kill(this);
            transform.DOScale(0.6f, 0.12f).SetId(this).SetLink(gameObject);
            _canvasGroup.DOFade(0f, 0.12f).SetId(this).SetLink(gameObject);
            _data = null;
        }

        private void OnDestroy()
        {
            CancelReveal();
        }

        /// <summary>
        /// If the line is still revealing: completes the line instantly and returns false.
        /// If the line is fully shown: advances to the next line or returns true to close the dialogue.
        /// </summary>
        public bool TryAdvance()
        {
            if (_data == null || _data.Replicas.Count == 0)
                return true;

            if (!_lineRevealComplete)
            {
                CompleteCurrentLineInstantly();
                return false;
            }

            if (_lineIndex < _data.Replicas.Count - 1)
            {
                _lineIndex++;
                ApplyCurrentLine();
                return false;
            }

            return true;
        }

        private void ApplyCurrentLine()
        {
            CancelReveal();

            if (_data == null || _data.Replicas.Count == 0 || _lineIndex < 0 || _lineIndex >= _data.Replicas.Count)
            {
                if (_speakerNameText != null) _speakerNameText.text = string.Empty;
                if (_lineText != null) _lineText.text = string.Empty;
                _fullLineText = string.Empty;
                _lineRevealComplete = true;
                return;
            }

            DialogueReplica replica = _data.Replicas[_lineIndex];
            NPCData npc = replica.NpcData;
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

            _fullLineText = replica.Text ?? string.Empty;
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

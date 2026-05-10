using DG.Tweening;
using UnityEngine;

namespace UI.PlayerHud
{
    public class NoteView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMPro.TMP_Text _noteText;

        public void ShowNote(string text)
        {
            _noteText.text = text;
            _canvasGroup.alpha = 0;
            transform.localScale = Vector3.one * 0.6f;

            DOTween.Kill(this);
            transform.DOScale(1, 0.2f).SetLink(gameObject);
            _canvasGroup.DOFade(1, 0.15f).SetLink(gameObject);
        }

        public void Hide()
        {
            DOTween.Kill(this);
            transform.DOScale(0.6f, 0.12f).SetLink(gameObject);
            _canvasGroup.DOFade(0, 0.12f).SetLink(gameObject);
        }
    }
}
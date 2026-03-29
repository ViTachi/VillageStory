using DG.Tweening;
using UnityEngine;

namespace UI.PlayerHud
{
    public class KeyHintInfoView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMPro.TMP_Text infoLabel;

        public void Initialize()
        {
            transform.localScale = Vector3.zero;
            canvasGroup.alpha = 1f;
        }

        public void ShowHintText(string text)
        {
            DOTween.Kill(transform);

            transform.localScale = Vector3.zero;
            transform.DOScale(1f, 0.15f)
                .SetEase(Ease.OutBack)
                .SetId(transform).SetLink(gameObject);
            infoLabel.text = text;
        }

        public void HideHintText()
        {
            DOTween.Kill(transform);

            transform.DOScale(0f, 0.15f)
                .SetId(transform).SetLink(gameObject);
        }
    }
}
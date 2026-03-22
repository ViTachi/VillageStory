using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIElement : MonoBehaviour
    {
        protected CanvasGroup canvasGroup;

        public virtual UniTask Show(bool immediately, CancellationToken ct)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }

        public virtual UniTask Hide(bool immediately, CancellationToken ct)
        {
            gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }
    }
}
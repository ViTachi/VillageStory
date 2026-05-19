using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.PlayerHud
{
    public class DialogueAnswersView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _answerButtonPrefab;

        private readonly List<Button> _spawnedButtons = new();
        private Action<int> _onSelected;

        private void Awake()
        {
            Hide();
        }

        public void ShowAnswers(IReadOnlyList<string> variants, Action<int> onSelected)
        {
            ClearButtons();
            _onSelected = onSelected;
            Show();

            for (int i = 0; i < variants.Count; i++)
            {
                int index = i;
                Button button = Instantiate(_answerButtonPrefab, transform);
                TMP_Text label = button.GetComponentInChildren<TMP_Text>(true);
                if (label != null)
                    label.text = variants[i];

                button.onClick.AddListener(() => OnVariantSelected(index));
                button.gameObject.AddComponent<DialogueAnswerButtonHoverSelect>();
                _spawnedButtons.Add(button);
            }

            SetupNavigation();
            FocusFirstButtonAsync().Forget();
        }

        public bool TrySubmitFocusedAnswer()
        {
            if (TryGetButtonUnderPointer(out Button pointerButton))
            {
                pointerButton.onClick.Invoke();
                return true;
            }

            GameObject selected = EventSystem.current?.currentSelectedGameObject;
            if (selected != null && selected.TryGetComponent(out Button selectedButton))
            {
                selectedButton.onClick.Invoke();
                return true;
            }

            return false;
        }

        public void Hide()
        {
            ClearButtons();
            _onSelected = null;
            EventSystem.current?.SetSelectedGameObject(null);

            DOTween.Kill(this);
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        private void Show()
        {
            DOTween.Kill(this);
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 0f;
            _canvasGroup.DOFade(1f, 0.15f).SetId(this).SetLink(gameObject);
        }

        private void SetupNavigation()
        {
            int count = _spawnedButtons.Count;
            for (int i = 0; i < count; i++)
            {
                Button button = _spawnedButtons[i];
                Navigation navigation = new Navigation { mode = Navigation.Mode.Explicit };
                navigation.selectOnUp = i > 0 ? _spawnedButtons[i - 1] : null;
                navigation.selectOnDown = i < count - 1 ? _spawnedButtons[i + 1] : null;
                navigation.selectOnLeft = button;
                navigation.selectOnRight = button;
                button.navigation = navigation;
            }
        }

        private async UniTaskVoid FocusFirstButtonAsync()
        {
            if (_spawnedButtons.Count == 0)
                return;

            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);

            EventSystem eventSystem = EventSystem.current;
            if (eventSystem == null)
                return;

            eventSystem.SetSelectedGameObject(_spawnedButtons[0].gameObject);
        }

        private void OnVariantSelected(int index)
        {
            _onSelected?.Invoke(index);
        }

        private static bool TryGetButtonUnderPointer(out Button button)
        {
            button = null;
            EventSystem eventSystem = EventSystem.current;
            if (eventSystem == null)
                return false;

            Vector2 position = UnityEngine.InputSystem.Mouse.current != null
                ? UnityEngine.InputSystem.Mouse.current.position.ReadValue()
                : (Vector2)UnityEngine.Input.mousePosition;

            var pointerData = new PointerEventData(eventSystem) { position = position };
            var results = new List<RaycastResult>();
            eventSystem.RaycastAll(pointerData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.TryGetComponent(out button))
                    return true;

                button = result.gameObject.GetComponentInParent<Button>();
                if (button != null)
                    return true;
            }

            return false;
        }

        private void ClearButtons()
        {
            foreach (Button button in _spawnedButtons)
            {
                if (button == null) continue;
                button.onClick.RemoveAllListeners();
                Destroy(button.gameObject);
            }

            _spawnedButtons.Clear();
        }
    }

    internal class DialogueAnswerButtonHoverSelect : MonoBehaviour, IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            EventSystem.current?.SetSelectedGameObject(gameObject);
        }
    }
}

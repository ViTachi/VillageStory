using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Screens
{
    public class SettingsTabController : MonoBehaviour
    {
        [Serializable]
        private class TabEntry
        {
            [field: SerializeField] public Button Button { get; private set; }
            [field: SerializeField] public GameObject Panel { get; private set; }
        }

        [SerializeField] private List<TabEntry> _tabs = new();
        [SerializeField] private int _defaultTabIndex;

        private readonly List<(Button button, UnityAction listener)> _listeners = new();

        private void Awake()
        {
            for (int i = 0; i < _tabs.Count; i++)
            {
                TabEntry tab = _tabs[i];
                if (tab.Button == null)
                    continue;

                int index = i;
                UnityAction listener = () => SelectTab(index);
                tab.Button.onClick.AddListener(listener);
                _listeners.Add((tab.Button, listener));
            }

            SelectTab(_defaultTabIndex);
        }

        private void OnDestroy()
        {
            foreach ((Button button, UnityAction listener) in _listeners)
            {
                if (button != null)
                    button.onClick.RemoveListener(listener);
            }

            _listeners.Clear();
        }

        public void SelectTab(int index)
        {
            if (_tabs.Count == 0)
                return;

            index = Mathf.Clamp(index, 0, _tabs.Count - 1);

            for (int i = 0; i < _tabs.Count; i++)
            {
                TabEntry tab = _tabs[i];
                if (tab.Panel != null)
                    tab.Panel.SetActive(i == index);
            }
        }
    }
}

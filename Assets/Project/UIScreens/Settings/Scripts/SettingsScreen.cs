using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class SettingsScreen : Screen<SettingsScreen.Parameters, object>
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _applyButton;

        [Header("Controls")]
        [SerializeField] private Slider _mouseSensitivitySlider;

        [Header("Graphics")]
        [SerializeField] private Slider _qualitySlider;
        [SerializeField] private Toggle _vSyncToggle;
        [SerializeField] private Slider _frameRateSlider;
        [SerializeField] private TMP_Text _qualityLabel;
        [SerializeField] private TMP_Text _frameRateLabel;

        [Header("Audio")]
        [SerializeField] private Slider _masterVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _sfxVolumeSlider;

        private ISettingsService _settingsService;
        private SettingsData _pendingSettings;
        private bool _isBinding;

        public event Action Closed;

        public class Parameters
        {
            public ISettingsService SettingsService;
        }

        public override async UniTask Show(Parameters parameters, bool immediately,
            CancellationToken ct, Action<object> callback = null)
        {
            await base.Show(parameters, immediately, ct, callback);

            _settingsService = parameters.SettingsService;
            _pendingSettings = CopySettings(_settingsService.Current);

            ConfigureSliders();
            BindUiFromSettings(_pendingSettings);
            SubscribeUiEvents();
            _backButton.onClick.AddListener(OnBackClicked);
            _applyButton.onClick.AddListener(OnApplyClicked);
        }

        public override UniTask Hide(bool immediately, CancellationToken ct)
        {
            UnsubscribeUiEvents();
            _backButton.onClick.RemoveListener(OnBackClicked);
            _applyButton.onClick.RemoveListener(OnApplyClicked);
            canvasGroup.interactable = false;
            return base.Hide(immediately, ct);
        }

        private void ConfigureSliders()
        {
            _qualitySlider.minValue = 0;
            _qualitySlider.maxValue = Mathf.Max(0, QualitySettings.names.Length - 1);
            _qualitySlider.wholeNumbers = true;

            _frameRateSlider.minValue = 30;
            _frameRateSlider.maxValue = 144;
            _frameRateSlider.wholeNumbers = true;

            _mouseSensitivitySlider.minValue = 0.5f;
            _mouseSensitivitySlider.maxValue = 10f;

            _masterVolumeSlider.minValue = 0f;
            _masterVolumeSlider.maxValue = 1f;
            _musicVolumeSlider.minValue = 0f;
            _musicVolumeSlider.maxValue = 1f;
            _sfxVolumeSlider.minValue = 0f;
            _sfxVolumeSlider.maxValue = 1f;
        }

        private void BindUiFromSettings(SettingsData settings)
        {
            _isBinding = true;

            _mouseSensitivitySlider.SetValueWithoutNotify(settings.MouseSensitivity);
            _qualitySlider.SetValueWithoutNotify(settings.QualityLevel);
            _vSyncToggle.SetIsOnWithoutNotify(settings.VSync);
            _frameRateSlider.SetValueWithoutNotify(settings.FrameRate);
            _masterVolumeSlider.SetValueWithoutNotify(settings.MasterVolume);
            _musicVolumeSlider.SetValueWithoutNotify(settings.MusicVolume);
            _sfxVolumeSlider.SetValueWithoutNotify(settings.SFXVolume);

            UpdateQualityLabel(settings.QualityLevel);
            UpdateFrameRateLabel(settings.FrameRate);

            _isBinding = false;
        }

        private void SubscribeUiEvents()
        {
            _mouseSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivityChanged);
            _qualitySlider.onValueChanged.AddListener(OnQualityChanged);
            _vSyncToggle.onValueChanged.AddListener(OnVSyncChanged);
            _frameRateSlider.onValueChanged.AddListener(OnFrameRateChanged);
            _masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            _sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
        }

        private void UnsubscribeUiEvents()
        {
            _mouseSensitivitySlider.onValueChanged.RemoveListener(OnMouseSensitivityChanged);
            _qualitySlider.onValueChanged.RemoveListener(OnQualityChanged);
            _vSyncToggle.onValueChanged.RemoveListener(OnVSyncChanged);
            _frameRateSlider.onValueChanged.RemoveListener(OnFrameRateChanged);
            _masterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);
            _musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
            _sfxVolumeSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);
        }

        private void OnMouseSensitivityChanged(float value)
        {
            if (_isBinding) return;
            _pendingSettings.MouseSensitivity = value;
        }

        private void OnQualityChanged(float value)
        {
            if (_isBinding) return;
            int qualityLevel = Mathf.RoundToInt(value);
            _pendingSettings.QualityLevel = qualityLevel;
            UpdateQualityLabel(qualityLevel);
        }

        private void OnVSyncChanged(bool value)
        {
            if (_isBinding) return;
            _pendingSettings.VSync = value;
        }

        private void OnFrameRateChanged(float value)
        {
            if (_isBinding) return;
            int frameRate = Mathf.RoundToInt(value);
            _pendingSettings.FrameRate = frameRate;
            UpdateFrameRateLabel(frameRate);
        }

        private void OnMasterVolumeChanged(float value)
        {
            if (_isBinding) return;
            _pendingSettings.MasterVolume = value;
        }

        private void OnMusicVolumeChanged(float value)
        {
            if (_isBinding) return;
            _pendingSettings.MusicVolume = value;
        }

        private void OnSfxVolumeChanged(float value)
        {
            if (_isBinding) return;
            _pendingSettings.SFXVolume = value;
        }

        private void OnApplyClicked()
        {
            CopySettings(_pendingSettings, _settingsService.Current);
            _settingsService.Save();
        }

        private void UpdateQualityLabel(int qualityLevel)
        {
            if (_qualityLabel == null)
                return;

            string[] names = QualitySettings.names;
            if (names.Length == 0)
            {
                _qualityLabel.text = qualityLevel.ToString();
                return;
            }

            qualityLevel = Mathf.Clamp(qualityLevel, 0, names.Length - 1);
            _qualityLabel.text = names[qualityLevel];
        }

        private void UpdateFrameRateLabel(int frameRate)
        {
            if (_frameRateLabel != null)
                _frameRateLabel.text = frameRate.ToString();
        }

        private void OnBackClicked()
        {
            Closed?.Invoke();
        }

        private static SettingsData CopySettings(SettingsData source)
        {
            var copy = new SettingsData();
            CopySettings(source, copy);
            return copy;
        }

        private static void CopySettings(SettingsData source, SettingsData target)
        {
            target.MouseSensitivity = source.MouseSensitivity;
            target.MasterVolume = source.MasterVolume;
            target.MusicVolume = source.MusicVolume;
            target.SFXVolume = source.SFXVolume;
            target.QualityLevel = source.QualityLevel;
            target.VSync = source.VSync;
            target.FrameRate = source.FrameRate;
        }
    }
}

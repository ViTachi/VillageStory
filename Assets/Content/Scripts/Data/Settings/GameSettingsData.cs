using System;
using UnityEngine;

[Serializable]
public class GameSettingsData
{
    [SerializeField] private float _soundVolume = 1.0f;
    [SerializeField] private float _musicVolume = 1.0f;
    [SerializeField] private float _sensitivity = 1.0f;
    [SerializeField] private SettingsLevel _currentSettingsLevel = SettingsLevel.Middle;

    public float SoundVolume => _soundVolume;
    public float MusicVolume => _musicVolume;
    public float Sensitivity => _sensitivity;
    public SettingsLevel CurrentSettingsLevel => _currentSettingsLevel;

    public enum SettingsLevel
    {
        VeryLow = 0, Low = 1, Middle = 2, High = 3
    }

    public void SetLevel(SettingsLevel level)
    {
        _currentSettingsLevel = level;
    }  

    public void SetSoundVolume(float volume)
    {
        _soundVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        _musicVolume = volume;
    }

    public void SetSensitivity(float sensitivity)
    {
        _sensitivity = sensitivity;
    }
}

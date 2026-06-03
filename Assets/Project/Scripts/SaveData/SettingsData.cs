using System;

[Serializable]
public class SettingsData
{
    public float MouseSensitivity = 5.0f;
    public float MasterVolume = 1.0f;
    public float MusicVolume = 1.0f;
    public float SFXVolume = 1.0f;
    public int QualityLevel = 1;
    public bool VSync = true;
    public int FrameRate = 90;
}

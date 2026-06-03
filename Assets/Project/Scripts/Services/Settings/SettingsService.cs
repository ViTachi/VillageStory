using UnityEngine;
using Zenject;

public class SettingsService : ISettingsService, IInitializable
{
    [Inject] private ISaveLoadService _saveLoadService;

    public SettingsData Current { get; private set; } = new();

    public void Initialize()
    {
        Load();
    }

    public void Load()
    {
        Current = _saveLoadService.Data.Settings ?? new SettingsData();
        Apply(Current);
    }

    public void Save()
    {
        _saveLoadService.Data.Settings = Current;
        _saveLoadService.Save();
        Apply(Current);
    }

    public void Apply(SettingsData settings)
    {
        QualitySettings.SetQualityLevel(settings.QualityLevel, true);
        QualitySettings.vSyncCount = settings.VSync ? 1 : 0;
        Application.targetFrameRate = settings.FrameRate;
        AudioListener.volume = settings.MasterVolume / 10f;
    }
}

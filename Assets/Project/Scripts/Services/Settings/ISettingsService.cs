public interface ISettingsService
{
    SettingsData Current { get; }

    void Load();
    void Save();
    void Apply(SettingsData settings);
}

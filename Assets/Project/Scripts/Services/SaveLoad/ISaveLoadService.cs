public interface ISaveLoadService
{
    SaveData Data { get; }

    void Load();
    void Save();
    bool HasSave();
    void DeleteSave();
}

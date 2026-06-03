using UnityEngine;
using Zenject;

public class SaveLoadService : ISaveLoadService, IInitializable
{
    public const string SaveFilePath = "save.json";

    [Inject] private IFileService _fileService;

    public SaveData Data { get; private set; } = new();

    public void Initialize()
    {
        Load();
    }

    public void Load()
    {
        string json = _fileService.LoadTextFromFile(SaveFilePath);
        if (string.IsNullOrEmpty(json))
        {
            Data = new SaveData();
            return;
        }

        Data = JsonUtility.FromJson<SaveData>(json) ?? new SaveData();
        EnsureSaveDataInitialized();
    }

    public void Save()
    {
        EnsureSaveDataInitialized();
        string json = JsonUtility.ToJson(Data, true);
        _fileService.SaveTextToFile(SaveFilePath, json);
    }

    public bool HasSave()
    {
        return !string.IsNullOrEmpty(_fileService.LoadTextFromFile(SaveFilePath));
    }

    public void DeleteSave()
    {
        _fileService.DeleteFile(SaveFilePath);
        Data = new SaveData();
    }

    private void EnsureSaveDataInitialized()
    {
        Data.Settings ??= new SettingsData();
        Data.GameScenario ??= new GameScenarioSaveData();
    }
}

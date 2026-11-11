using System;

[Serializable]
public class SaveData
{
    public int Version = 1;
    public SettingsData Settings = new();
    public GameScenarioSaveData GameScenario = new();
}

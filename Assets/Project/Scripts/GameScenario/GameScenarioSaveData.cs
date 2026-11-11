using System;
using System.Collections.Generic;

[Serializable]
public class GameScenarioSaveData
{
    public List<string> ScenarioEvents = new();

    public void AddScenarioEvent(GameScenarioEvent scenarioEvent)
    {
        if (scenarioEvent == null || string.IsNullOrEmpty(scenarioEvent.EventName))
            return;

        if (ScenarioEvents.Contains(scenarioEvent.EventName))
            return;

        ScenarioEvents.Add(scenarioEvent.EventName);
    }

    public bool ContainsEvent(string eventName)
    {
        return !string.IsNullOrEmpty(eventName) && ScenarioEvents.Contains(eventName);
    }
}

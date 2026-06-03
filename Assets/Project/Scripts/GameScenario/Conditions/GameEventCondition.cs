using System;
using UnityEngine;

[Serializable]
public class GameEventCondition : IGameCondition
{
    [field: SerializeField] public GameScenarioEvent Event { get; private set; }

    public bool IsMet(SaveData saveData)
    {
        if (Event == null || string.IsNullOrEmpty(Event.EventName))
            return false;

        return saveData?.GameScenario != null
               && saveData.GameScenario.ContainsEvent(Event.EventName);
    }
}

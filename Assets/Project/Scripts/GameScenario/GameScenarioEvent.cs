using UnityEngine;

[CreateAssetMenu(menuName = "Scenario/Event", fileName = "Event")]
public class GameScenarioEvent : ScriptableObject
{
    [field: SerializeField] public string EventName { get; private set; }

    private void OnValidate()
    {
        EventName = name;
    }
}

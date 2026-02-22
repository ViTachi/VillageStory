using UnityEngine;

[CreateAssetMenu(menuName = "Data/GameSettings", fileName = "GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private GameSettingsData _settings;

    public GameSettingsData Settings => _settings;
}

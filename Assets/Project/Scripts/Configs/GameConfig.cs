using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
public class GameConfig : ScriptableObject
{
    [field: SerializeField] public int TargetFPS { get; private set; } = 90;
    [field: SerializeField] public GameObject PlayerPrefab { get; private set; }
}

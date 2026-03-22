using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
public class GameConfig : ScriptableObject
{
    [field: SerializeField] public GameObject PlayerPrefab { get; private set; }
}

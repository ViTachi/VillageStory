using UnityEngine;

[CreateAssetMenu(menuName = "Data/GameItem", fileName = "Item")]
public class GameItemData : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private string _itemName;

    public string Id => _id;
    public string ItemName => _itemName;
}

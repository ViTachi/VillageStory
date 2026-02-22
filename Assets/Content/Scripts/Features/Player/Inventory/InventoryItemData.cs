using System;

public class InventoryItemData
{
    public string Id => _id;
    public Guid Guid => _guid;

    private readonly string _id;
    private readonly Guid _guid;

    public InventoryItemData(string id)
    {
        _id = id;
        _guid = Guid.NewGuid();
    }
}

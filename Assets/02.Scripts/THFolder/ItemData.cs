[System.Serializable]
public class ItemData
{
    public string itemName;
    public int itemID;

    public ItemChip.Item itemType;

    public ItemData(string name, int id, ItemChip.Item type)
    {
        itemName = name;
        itemID = id;
        itemType = type;



    }
}
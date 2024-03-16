[System.Serializable]
public class ItemData
{
    public string itemName;
    public int itemID;

    public ItemChip.Item itemType;

    public ItemData(string name, int id)
    {
        itemName = name;
        itemID = id;

        
    }
}
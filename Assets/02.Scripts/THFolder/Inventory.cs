using System;
using System.Collections.Generic;

public class Inventory
{
    public List<ItemChip> items = new List<ItemChip>();

    public void AddItem(ItemChip item)
    {
        items.Add(item);
    }

    public void RemoveItem(ItemChip item)
    {
        items.Remove(item);
    }

    public void ListItems()
    {
        foreach (ItemChip item in items)
        {
        }
    }
}

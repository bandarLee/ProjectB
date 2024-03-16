using System;
using System.Collections;

using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;
public class Inventory:MonoBehaviour
{
    private static Inventory m_instance;
    public List<ItemData> items = new List<ItemData>();

    private Dictionary<ItemChip.Item, List<ItemData>> sortedItems = new Dictionary<ItemChip.Item, List<ItemData>>();

    public static Inventory Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<Inventory>();
            }

            return m_instance;
        }
    }
    public void AddItem(ItemData item)
    {
        items.Add(item);
    }

    public void RemoveItem(ItemData item)
    {
        items.Remove(item);
    }

    public void ListItems()
    {
        foreach (ItemData item in items)
        {
            Debug.Log("Item Name: " + item.itemName + item.itemID);
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L)) {
            ListItems();

        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            SortItemsByType();

        }
    }

    public void SortItemsByType()
    {
        sortedItems.Clear();

        foreach (var item in items)
        {
            var itemType = GetItemTypeFromID(item.itemID);
            if (!sortedItems.ContainsKey(itemType))
            {
                sortedItems[itemType] = new List<ItemData>();
            }
            sortedItems[itemType].Add(item);
        }

        ReassignSortedItemsToItemsList();

    }//아이템 타입별로 묶어준다!
    private ItemChip.Item GetItemTypeFromID(int id)
    {
        if (id >= 1000 && id < 2000) return ItemChip.Item.Health;
        if (id >= 2000 && id < 3000) return ItemChip.Item.Stat;
        if (id >= 3000) return ItemChip.Item.Weapon;
        return ItemChip.Item.Health;
    }
    private void ReassignSortedItemsToItemsList()
    {
        items.Clear();

        foreach (var keyValuePair in sortedItems.OrderBy(kvp => kvp.Key))
        {
            foreach (var item in keyValuePair.Value)
            {
                items.Add(item);
            }
        }
    }//묶은 아이템을 정렬해서 List에 배치한다!




}

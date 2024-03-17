using System;
using System.Collections;

using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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
        int judgeitemtype = id / 1000;

        switch (judgeitemtype)
        {
            case 10:
                return ItemChip.Item.HealthSmall;
            case 11:
                return ItemChip.Item.HealthMedium;
            case 12:
                return ItemChip.Item.HealthLarge;

            case 20:
                return ItemChip.Item.statSTRSmall;
            case 21:
                return ItemChip.Item.statSTRMedium;
            case 22:
                return ItemChip.Item.statSTRLarge;
            case 23:
                return ItemChip.Item.statATKSmall;
            case 24:
                return ItemChip.Item.statATKMedium;
            case 25:
                return ItemChip.Item.statATKLarge;
            case 26:
                return ItemChip.Item.statDMGSmall;
            case 27:
                return ItemChip.Item.statDMGMedium;
            case 28:
                return ItemChip.Item.statDMGLarge;
            case 29:
                return ItemChip.Item.statDEXSmall;
            case 30:
                return ItemChip.Item.statDEXMedium;
            case 31:
                return ItemChip.Item.statDEXLarge;
            case 32:
                return ItemChip.Item.statSpeedSmall;
            case 33:
                return ItemChip.Item.statSpeedMedium;
            case 34:
                return ItemChip.Item.statSpeedLarge;

            case 40:
                return ItemChip.Item.WeaponA1;
            case 41:
                return ItemChip.Item.WeaponA2;
            case 42:
                return ItemChip.Item.WeaponA3;
            case 43:
                return ItemChip.Item.WeaponB1;
            case 44:
                return ItemChip.Item.WeaponB2;
            case 45:
                return ItemChip.Item.WeaponB3;
            case 46:
                return ItemChip.Item.WeaponC1;
            case 47:
                return ItemChip.Item.WeaponC2;
            case 48:
                return ItemChip.Item.WeaponC3;
            default:
                return 0;
        }

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

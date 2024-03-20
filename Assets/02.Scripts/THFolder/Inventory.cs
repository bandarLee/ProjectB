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
public class Inventory : MonoBehaviour
{
    // 인벤토리 인스턴스 변수
    private static Inventory m_instance;

    // 인벤토리에 포함된 아이템들을 담는 리스트
    public List<ItemData> items = new List<ItemData>();

    // 아이템을 종류별로 분류하기 위한 딕셔너리
    private Dictionary<ItemChip.Item, List<ItemData>> sortedItems = new Dictionary<ItemChip.Item, List<ItemData>>();

    //public InventorySlot[] inventorySlots;
    public List<InventorySlot> slots = new List<InventorySlot>();

    public GameObject RemoveSelect;

    private InventorySlot _inventorySlot;
    private InventorySlot selectedSlot;

   

    private void Start()
    {
        RemoveSelect.SetActive(false);
        _inventorySlot = GetComponent<InventorySlot>();

    }
    // 인벤토리 인스턴스 접근을 위한 프로퍼티
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

    // 아이템을 인벤토리에 추가하는 메서드
    public void AddItem(ItemData item)
    {
        items.Add(item);
    }

    // 아이템을 인벤토리에서 제거하는 메서드
    public void RemoveItem(ItemData item)
    {
        items.Remove(item);
    }


    // 인벤토리에 있는 모든 아이템을 콘솔에 출력하는 메서드
    public void ListItems()
    {
        foreach (InventorySlot inventorySlot in slots) 
        {
            inventorySlot.gameObject.SetActive(false);
        }
        for (int i = 0; i < items.Count; i++)
        {
            if (i < slots.Count)
            {
                slots[i].gameObject.SetActive(true);
                slots[i].ItemNameText.text = items[i].itemName;
                slots[i]._c = items[i];

                // 아이템 ID에 따라 다른 이미지를 할당하는 로직을 추가합니다.
                int itemIdDivision = items[i].itemID / 10000;
                switch (itemIdDivision)
                {
                    case 1:
                        slots[i].ItemImage1.gameObject.SetActive(true);
                        slots[i].ItemImage2.gameObject.SetActive(false);
                        slots[i].ItemImage3.gameObject.SetActive(false);
                        break;
                    case 2:
                    case 3:
                        slots[i].ItemImage1.gameObject.SetActive(false);
                        slots[i].ItemImage2.gameObject.SetActive(true);
                        slots[i].ItemImage3.gameObject.SetActive(false);
                        break;
                    case 4:
                        slots[i].ItemImage1.gameObject.SetActive(false);
                        slots[i].ItemImage2.gameObject.SetActive(false);
                        slots[i].ItemImage3.gameObject.SetActive(true);
                        break;
                    default:
                        break;
                }
            }
        }
        /*foreach (ItemData item in items)
        {
            Debug.Log("Item Name: " + item.itemName + item.itemID);
        }*/
    }
    
    public void OnClickRemoveSelectButton() 
    {
        RemoveSelect.SetActive(true);
    }
    public void OnClickYesButton() 
    {
        if (selectedSlot != null && items.Contains(selectedSlot._c)) 
        {
            RemoveItem(selectedSlot._c); // 선택한 슬롯의 아이템 삭제
            selectedSlot = null; // 선택한 슬롯 참조 제거
        }
        RemoveSelect.SetActive(false);
        ListItems();
    }
    public void OnClickNoButton() 
    {
        RemoveSelect.SetActive(false);
    }
    public void OnClickSortButton() 
    {
        SortItemsByType();
    }
    // 키 입력을 감지하여 아이템 리스트 출력 또는 아이템 종류별로 분류하는 메서드 호출
    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape)) || (Input.GetKeyDown(KeyCode.M)) || (Input.GetKeyDown(KeyCode.I)))
        {
            ListItems();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            SortItemsByType();
        }
    }

    // 아이템을 종류별로 분류하는 메서드
    public void SortItemsByType()
    {
        // sortedItems 컬렉션을 초기화
        sortedItems.Clear();

        // items 컬렉션의 각 아이템에 대하여 반복 처리함
        foreach (var item in items)
        {
            // 아이템의 ID를 통해 아이템 타입을 가져옴
            var itemType = GetItemTypeFromID(item.itemID);
            // sortedItems에 해당 아이템 타입의 키가 없으면, 새로운 리스트를 만들어서 추가함
            if (!sortedItems.ContainsKey(itemType))
            {
                sortedItems[itemType] = new List<ItemData>();
            }
            // 해당 아이템 타입의 리스트에 현재 아이템을 추가
            sortedItems[itemType].Add(item);
        }

        // sortedItems에 정렬된 아이템들을 다시 items 리스트에 재배치하는 함수를 호출
        ReassignSortedItemsToItemsList();
        ListItems();



    }//아이템 타입별로 묶어준다!

    public void SetSelectedSlot(InventorySlot slot)
    {
        selectedSlot = slot;
    }
    // 아이템 ID를 통해 아이템의 종류를 반환하는 메서드
    private ItemChip.Item GetItemTypeFromID(int id)
    {
        int judgeitemtype = id / 1000;

        switch (judgeitemtype)
        {
            // 아이템 종류에 따라 반환하는 값이 다름
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

    // 분류된 아이템을 다시 정렬하여 리스트에 할당하는 메서드
    private void ReassignSortedItemsToItemsList()
    {
        // items 리스트를 초기화, 기존에 있던 모든 아이템들을 제거
        items.Clear();

        // sortedItems 딕셔너리를 키(key) 기준으로 오름차순 정렬
        // 각 키와 값을 keyValuePair 변수에 저장하며 반복
        foreach (var keyValuePair in sortedItems.OrderBy(kvp => kvp.Key))
        {
            // 현재 keyValuePair의 Value(아이템 리스트)를 순회
            foreach (var item in keyValuePair.Value)
            {
                // 순회하는 아이템을 items 리스트에 추가
                items.Add(item);
            }
        }
    }//묶은 아이템을 정렬해서 List에 배치한다!


   

}

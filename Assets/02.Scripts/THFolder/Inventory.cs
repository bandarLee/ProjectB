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
using static PlayerStat;
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
    public List<EnforceSlot> enforceslots = new List<EnforceSlot>();
    public List<EnforceSlot> equipslots = new List<EnforceSlot>();

    private List<ItemData> selectedItems = new List<ItemData>();
    private HashSet<int> selectedItemIDs = new HashSet<int>();

    private int currentEquipSlotIndex = 0;

    public GameObject RemoveSelect;


    private InventorySlot _inventorySlot;
    private InventorySlot selectedSlot;


    private HashSet<int> allocatedItemIDs = new HashSet<int>();

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
        foreach (InventorySlot slots in slots) 
        {
            slots.gameObject.SetActive(false);
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
    public void ListEnforceItems()
    {
        foreach (EnforceSlot enforceSlot in enforceslots)
        {
            enforceSlot.gameObject.SetActive(false);
        }
        for (int i = 0; i < items.Count; i++)
        {
            if (i < enforceslots.Count)
            {
                enforceslots[i].gameObject.SetActive(true);
                enforceslots[i]._c = items[i];
                enforceslots[i].ItemNameText.text = items[i].itemName;

                // 아이템 ID에 따라 다른 이미지를 할당하는 로직을 추가합니다.
                int itemIdDivision = items[i].itemID / 10000;
                switch (itemIdDivision)
                {
                    case 1:
                        enforceslots[i].ItemImage1.gameObject.SetActive(true);
                        enforceslots[i].ItemImage2.gameObject.SetActive(false);
                        enforceslots[i].ItemImage3.gameObject.SetActive(false);
                        break;
                    case 2:
                    case 3:
                        enforceslots[i].ItemImage1.gameObject.SetActive(false);
                        enforceslots[i].ItemImage2.gameObject.SetActive(true);
                        enforceslots[i].ItemImage3.gameObject.SetActive(false);
                        break;
                    case 4:
                        enforceslots[i].ItemImage1.gameObject.SetActive(false);
                        enforceslots[i].ItemImage2.gameObject.SetActive(false);
                        enforceslots[i].ItemImage3.gameObject.SetActive(true);
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
                return ItemChip.Item.statDMGSmall;
            case 24:
                return ItemChip.Item.statDMGMedium;
            case 25:
                return ItemChip.Item.statDMGLarge;
            case 26:
                return ItemChip.Item.statDEXSmall;
            case 27:
                return ItemChip.Item.statDEXMedium;
            case 28:
                return ItemChip.Item.statDEXLarge;
            case 29:
                return ItemChip.Item.statSpeedSmall;
            case 30:
                return ItemChip.Item.statSpeedMedium;
            case 31:
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
    public void AddItemToNextEquipSlot(ItemData itemData)
    {
        // 아이템이 이미 할당되었는지 확인
        if (allocatedItemIDs.Contains(itemData.itemID))
        {
            Debug.Log("이미 할당된 아이템입니다.");
            return; // 메서드 종료
        }

        if (currentEquipSlotIndex < equipslots.Count)
        {
            EnforceSlot currentSlot = equipslots[currentEquipSlotIndex];
            currentSlot.UpdateItemImage(itemData); // 현재 슬롯에 아이템 이미지 업데이트
            currentEquipSlotIndex++; // 다음 슬롯으로 인덱스 업데이트

            // 아이템 ID를 할당된 아이템의 HashSet에 추가
            allocatedItemIDs.Add(itemData.itemID);
        }
        else
        {
            // 모든 슬롯이 차있는 경우, 오류 메시지 또는 처리 로직
            Debug.Log("모든 강화 슬롯이 차있습니다.");
        }
    }
    public void ResetAllEquipSlots()
    {
        foreach (var slot in equipslots)
        {
            slot.ResetSlot(); // 각 EnforceSlot 내의 ResetSlot 메서드 호출
        }
        currentEquipSlotIndex = 0; // 인덱스 초기화
        allocatedItemIDs.Clear();

    }

    public void DebugEquippedItemsAndApplyEffects()
    {
        bool hasEmptySlot = false; // 비어있는 슬롯이 있는지 추적
        PlayerStat playerStat = PlayerStat.Instance;
        List<ItemData> usedItems = new List<ItemData>(); // 사용된 아이템을 추적하는 리스트
        List<KeyValuePair<int, StatChangeLog>> changesToApply = new List<KeyValuePair<int, StatChangeLog>>();


        bool[] hasWeaponASet = { false, false, false };
        bool[] hasWeaponBSet = { false, false, false };
        int statChipsCount = 0; // 스탯 칩 개수를 추적

        foreach (var itemId in playerStat.statChangeLogs.Keys.ToList())
        {
            foreach (var slot in equipslots)
            {
                if (slot._c != null )
                {
                    playerStat.RemoveStatChange(itemId);
                }
            }
        }
        foreach (var slot in equipslots)
        {
            if (slot._c != null)
            {
                usedItems.Add(slot._c);

                Debug.Log("Equipped Item: " + slot._c.itemName + ", ID: " + slot._c.itemID);

                // 아이템에 따른 스탯 효과 적용
                StatChangeLog change = new StatChangeLog();
                switch (GetItemTypeFromID(slot._c.itemID))
                {
                    case ItemChip.Item.WeaponA1: hasWeaponASet[0] = true;  break;
                    case ItemChip.Item.WeaponA2: hasWeaponASet[1] = true;  break;
                    case ItemChip.Item.WeaponA3: hasWeaponASet[2] = true; break;
                    case ItemChip.Item.WeaponB1: hasWeaponBSet[0] = true; break;
                    case ItemChip.Item.WeaponB2: hasWeaponBSet[1] = true; break;
                    case ItemChip.Item.WeaponB3: hasWeaponBSet[2] = true; break;


                    case ItemChip.Item.statSTRSmall:
                        change = new StatChangeLog { strChange = 0.1f };
                        statChipsCount++;
                        break;
                    case ItemChip.Item.statSTRMedium:
                        change = new StatChangeLog { strChange = 0.2f };
                        statChipsCount++;

                        break;
                    case ItemChip.Item.statSTRLarge:
                        change = new StatChangeLog { strChange = 0.3f };
                        statChipsCount++;

                        break;

                    case ItemChip.Item.statDMGSmall:
                        change = new StatChangeLog { dmgChange = 0.1f };
                        statChipsCount++;

                        break;
                    case ItemChip.Item.statDMGMedium:
                        change = new StatChangeLog { dmgChange = 0.2f };
                        statChipsCount++;

                        break;
                    case ItemChip.Item.statDMGLarge:
                        change = new StatChangeLog { dmgChange = 0.3f };
                        statChipsCount++;

                        break;

                    case ItemChip.Item.statDEXSmall:
                        change = new StatChangeLog { dexChange = 0.1f };
                        statChipsCount++;

                        break;
                    case ItemChip.Item.statDEXMedium:
                        change = new StatChangeLog { dexChange = 0.2f };
                        statChipsCount++;

                        break;
                    case ItemChip.Item.statDEXLarge:
                        change = new StatChangeLog { dexChange = 0.3f };
                        statChipsCount++;

                        break;

                    case ItemChip.Item.statSpeedSmall:
                        change = new StatChangeLog { speedChange = 0.1f };
                        statChipsCount++;

                        break;
                    case ItemChip.Item.statSpeedMedium:
                        change = new StatChangeLog { speedChange = 0.2f };
                        statChipsCount++;

                        break;
                    case ItemChip.Item.statSpeedLarge:
                        change = new StatChangeLog { speedChange = 0.3f };
                        statChipsCount++;

                        break;

                }
                int itemId = slot._c.itemID; // 혹은 다른 고유 식별자 사용
                changesToApply.Add(new KeyValuePair<int, StatChangeLog>(itemId, change));


            }
            else
            {
                hasEmptySlot = true;
                Debug.Log("Equipped Slot is empty");
            }
        }
        bool isASetComplete = hasWeaponASet.All(has => has);
        bool isBSetComplete = hasWeaponBSet.All(has => has);
        if (hasEmptySlot)
        {
            Debug.Log("칩 세 개를 전부 장착해야 합니다.");
        }
        else
        {
            Debug.Log("이미 장착한 칩이 있을 경우, 장착한 칩은 삭제됩니다.");
        }
        if (!hasEmptySlot)
        {
            if (statChipsCount == 3)
            {
                foreach (var pair in changesToApply)
                {
                    playerStat.ApplyStatChange(pair.Key, pair.Value);
                }
            }
            if (statChipsCount == 3 || statChipsCount == 0)
            {
                foreach (var usedItem in usedItems)
                {
                    items.Remove(usedItem); // 인벤토리에서 아이템 제거
                }
            }
            if(isASetComplete)
            {
                PlayerAttack.Instance.ChangeWeapon(1);
            }
            if (isBSetComplete)
            {
                PlayerAttack.Instance.ChangeWeapon(2);

            }
        }
        ListItems(); // UI 업데이트
        ListEnforceItems();
        // 스탯 변경 후 UI 업데이트 등
    }

}





using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChip : MonoBehaviour
{
    public enum Item // 아이템 종류
    {
        HealthSmall,
        HealthMedium,
        HealthLarge,

        statSTRSmall,
        statSTRMedium,
        statSTRLarge,

        statDMGSmall,
        statDMGMedium,
        statDMGLarge,
        statDEXSmall,
        statDEXMedium,
        statDEXLarge,
        statSpeedSmall,
        statSpeedMedium,
        statSpeedLarge,


        WeaponA1,
        WeaponA2, 
        WeaponA3,

        WeaponB1, 
        WeaponB2,
        WeaponB3,



    }

    // 아이템 종류, 이름, ID
    public Item item;
    public string itemName;
    public int itemID;


    private void Start()
    {
        AssignItemID();
        SetItemName();
    }

    // 아이템 ID를 할당하는 메서드
    void AssignItemID()
    {
        // 아이템 ID를 ItemIDManager를 통해 가져옴
        itemID = ItemIDManager.Instance.GetNextID(item);

    }

    // 아이템 이름을 설정하는 메서드
    void SetItemName()
    {
        // 아이템 종류에 따라 이름 설정
        switch (item)
        {
            case Item.HealthSmall:
                itemName = "체력 회복칩[소]";
                break;
            case Item.HealthMedium:
                itemName = "체력 회복칩[중]";
                break;
            case Item.HealthLarge:
                itemName = "체력 회복칩[대]";
                break;

            case Item.statSTRSmall:
                itemName = "STR 강화칩[Lv1]";
                break;
            case Item.statSTRMedium:
                itemName = "STR 강화칩[Lv2]";
                break;
            case Item.statSTRLarge:
                itemName = "STR 강화칩[Lv3]";
                break;



            case Item.statDMGSmall:
                itemName = "DMG 강화칩[Lv1]";
                break;
            case Item.statDMGMedium:
                itemName = "DMG 강화칩[Lv2]";
                break;
            case Item.statDMGLarge:
                itemName = "DMG 강화칩[Lv3]";
                break;
            case Item.statDEXSmall:
                itemName = "DEX 강화칩[Lv1]";
                break;
            case Item.statDEXMedium:
                itemName = "DEX 강화칩[Lv2]";
                break;
            case Item.statDEXLarge:
                itemName = "DEX 강화칩[Lv3]";
                break;
            case Item.statSpeedSmall:
                itemName = "Speed 강화칩[Lv1]";
                break;
            case Item.statSpeedMedium:
                itemName = "Speed 강화칩[Lv2]";
                break;
            case Item.statSpeedLarge:
                itemName = "Speed 강화칩[Lv3]";
                break;
            case Item.WeaponA1:
                itemName = "< A >무기 도안[1]";
                break;
            case Item.WeaponA2:
                itemName = "< A >무기 도안[2]";
                break;
            case Item.WeaponA3:
                itemName = "< A >무기 도안[3]";
                break;

            case Item.WeaponB1:
                itemName = "< B >무기 도안[1]";
                break;
            case Item.WeaponB2:
                itemName = "< B >무기 도안[2]";
                break;
            case Item.WeaponB3:
                itemName = "< B >무기 도안[3]";
                break;



            default:
                itemName = "알 수 없는 아이템";
                break;
        }
    }
    private void OnTriggerEnter(Collider Player)
    {
        if (Player.CompareTag("Player"))
        {
            // 아이템 데이터 생성

            switch (item)
            {
                case Item.HealthSmall: 
                    PlayerStat.Instance.SmallPotion++;
                    break;

                case Item.HealthMedium:
                    PlayerStat.Instance.MediumPotion++;
                    break;
                case Item.HealthLarge:
                    PlayerStat.Instance.LargePotion++;

                    break;
                default:
                    ItemData newItem = new ItemData(itemName, itemID, item);

                    Inventory.Instance.AddItem(newItem);
                    break;
            }
            
            Destroy(gameObject);

        }
    }
}

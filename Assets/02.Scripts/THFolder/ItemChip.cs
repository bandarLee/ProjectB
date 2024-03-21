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

        WeaponC1, 
        WeaponC2,
        WeaponC3

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
            ItemData newItem = new ItemData(itemName, itemID, item);

            // 인벤토리에 아이템 추가
            Inventory.Instance.AddItem(newItem);
            
            // 아이템 게임 오브젝트 제거
            Destroy(gameObject);





            /*if (item == Item.Health)
            {
                itemName = "체력 회복 칩(중)";
                *//*       if (PlayerStat.instance.playerhealth <= PlayerStat.instance.playermaxhealth - 1)
                       {
                           PlayerStat.instance.playerhealth = PlayerStat.instance.playerhealth + 1f;
                       }
                       else
                       {
                           PlayerStat.instance.playerhealth = PlayerStat.instance.playermaxhealth;

                       }
                       PlayerStat.instance.UpdateHealthBar();*//*
                ItemData newItem = new ItemData(gameObject.name, item);

                // Inventory에 아이템 데이터 객체를 추가
                Inventory.Instance.AddItem(newItem);
                Destroy(this.gameObject);


            }
            if (item == Item.Stat)
            {
                itemName = "근력 강화 칩(Lv 1)";

                *//*                PlayerStat.instance.str = PlayerStat.instance.str + 0.1f;
                *//*
                Destroy(this.gameObject);

            }
            if (item == Item.Weapon)
            {
                itemName = "무기 도안 칩(불의 검1)";

                Destroy(this.gameObject);
            } 
        }*/
        }
    }
}

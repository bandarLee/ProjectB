using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChip : MonoBehaviour
{
    public enum Item
    {
        Health,
        Stat,
        Weapon
    }
    public Item item;
    public string itemName;
    public int itemID;

    private static int healthID = 1000;
    private static int statID = 2000;
    private static int weaponID = 3000;
    private void Start()
    {
        AssignItemID();
        SetItemName();
    }
    void AssignItemID()
    {
        switch (item)
        {
            case Item.Health:
                itemID = healthID++;
                break;
            case Item.Stat:
                itemID = statID++;
                break;
            case Item.Weapon:
                itemID = weaponID++;
                break;
        }
    }
    void SetItemName()
    {
        // 아이템 이름 설정 로직
    }
    private void OnTriggerEnter(Collider Player)
    {
        if (Player.CompareTag("Player"))
        {
            string itemName = "무슨무슨아이템"; 
            ItemData newItem = new ItemData(itemName, itemID);

            Inventory.Instance.AddItem(newItem);
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

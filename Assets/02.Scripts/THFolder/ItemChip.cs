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
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider Player)
    {
        if (Player.CompareTag("Player"))
        {
            if (item == Item.Health)
            {
                if (PlayerStat.instance.playerhealth <= PlayerStat.instance.playermaxhealth - 1)
                {
                    PlayerStat.instance.playerhealth = PlayerStat.instance.playerhealth + 1f;
                }
                else
                {
                    PlayerStat.instance.playerhealth = PlayerStat.instance.playermaxhealth;

                }
                PlayerStat.instance.UpdateHealthBar();

                Destroy(this.gameObject);

            }
            if (item == Item.Stat)
            {
                PlayerStat.instance.str = PlayerStat.instance.str + 0.1f;
                Destroy(this.gameObject);

            }
            if (item == Item.Weapon)
            {
                Destroy(this.gameObject);
            } 
        }
    }
}

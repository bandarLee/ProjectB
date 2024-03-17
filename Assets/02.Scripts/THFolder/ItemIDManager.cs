using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIDManager : MonoBehaviour
{
    public static ItemIDManager Instance { get; private set; }

    private Dictionary<ItemChip.Item, int> currentIDs = new Dictionary<ItemChip.Item, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeIDs();


        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void InitializeIDs()
    {
        currentIDs = new Dictionary<ItemChip.Item, int>()
        {
            { ItemChip.Item.HealthSmall, 10000 },
            { ItemChip.Item.HealthMedium, 11000 },
            { ItemChip.Item.HealthLarge, 12000 },

            { ItemChip.Item.statSTRSmall, 20000 },
            { ItemChip.Item.statSTRMedium, 21000 },
            { ItemChip.Item.statSTRLarge, 22000 },
            { ItemChip.Item.statATKSmall, 23000 },
            { ItemChip.Item.statATKMedium, 24000 },
            { ItemChip.Item.statATKLarge, 25000 },
            { ItemChip.Item.statDMGSmall, 26000 },
            { ItemChip.Item.statDMGMedium, 27000 },
            { ItemChip.Item.statDMGLarge, 28000 },
            { ItemChip.Item.statDEXSmall, 29000 },
            { ItemChip.Item.statDEXMedium, 30000 },
            { ItemChip.Item.statDEXLarge, 31000 },
            { ItemChip.Item.statSpeedSmall, 32000 },
            { ItemChip.Item.statSpeedMedium, 33000 },
            { ItemChip.Item.statSpeedLarge, 34000 },

            { ItemChip.Item.WeaponA1, 40000 },
            { ItemChip.Item.WeaponA2, 41000 },
            { ItemChip.Item.WeaponA3, 42000 },
            { ItemChip.Item.WeaponB1, 43000 },
            { ItemChip.Item.WeaponB2, 44000 },
            { ItemChip.Item.WeaponB3, 45000 },
            { ItemChip.Item.WeaponC1, 46000 },
            { ItemChip.Item.WeaponC2, 47000 },
            { ItemChip.Item.WeaponC3, 48000 }
        };
    }
    public int GetNextID(ItemChip.Item itemType)
    {
        if (!currentIDs.ContainsKey(itemType))
        {
            Debug.LogError("Unknown item type: " + itemType);
            return -1;
        }

        int nextID = currentIDs[itemType]++;
        return nextID;
    }
}

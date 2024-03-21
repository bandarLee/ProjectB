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
            DontDestroyOnLoad(gameObject); // 다른 씬으로 이동해도 파괴 안됨

            // 아이템 ID를 초기화하는 메서드 호출
            InitializeIDs();


        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 아이템 ID를 초기화하는 메서드
    private void InitializeIDs()
    {
        // 아이템 종류별로 초기 아이템 ID를 설정
        currentIDs = new Dictionary<ItemChip.Item, int>()
        {
            // 각 아이템에 대한 초기 아이템 ID 할당
            { ItemChip.Item.HealthSmall, 10000 },
            { ItemChip.Item.HealthMedium, 11000 },
            { ItemChip.Item.HealthLarge, 12000 },

            { ItemChip.Item.statSTRSmall, 20000 },
            { ItemChip.Item.statSTRMedium, 21000 },
            { ItemChip.Item.statSTRLarge, 22000 },

            { ItemChip.Item.statDMGSmall, 23000 },
            { ItemChip.Item.statDMGMedium, 24000 },
            { ItemChip.Item.statDMGLarge, 25000 },
            { ItemChip.Item.statDEXSmall, 26000 },
            { ItemChip.Item.statDEXMedium, 27000 },
            { ItemChip.Item.statDEXLarge, 28000 },
            { ItemChip.Item.statSpeedSmall, 29000 },
            { ItemChip.Item.statSpeedMedium, 30000 },
            { ItemChip.Item.statSpeedLarge, 31000 },

            { ItemChip.Item.WeaponA1, 40000 },
            { ItemChip.Item.WeaponA2, 41000 },
            { ItemChip.Item.WeaponA3, 42000 },
            { ItemChip.Item.WeaponB1, 43000 },
            { ItemChip.Item.WeaponB2, 44000 },
            { ItemChip.Item.WeaponB3, 45000 },

        };
    }

    // 다음에 사용할 아이템 ID를 반환하는 메서드
    public int GetNextID(ItemChip.Item itemType)
    {
        // 딕셔너리에 해당 아이템 종류가 없는 경우 오류를 출력하고 -1을 반환
        if (!currentIDs.ContainsKey(itemType))
        {
            Debug.LogError("Unknown item type: " + itemType);
            return -1;
        }

        // 해당 아이템 종류에 대한 아이템 ID를 증가시킨 후 반환
        int nextID = currentIDs[itemType]++;
        return nextID;
    }
}

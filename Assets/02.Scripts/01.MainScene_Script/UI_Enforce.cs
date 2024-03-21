using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Enforce : MonoBehaviour
{
    public Image EnforceImage;
    public TextMeshProUGUI EnforceTextUI;
    public List<EnforceSlot> enforceSlots;



    private void Awake()
    {
      
    }
    public void EnforceOpen()
    {
        gameObject.SetActive(true);
        Inventory.Instance.ListEnforceItems();
        Cursor.lockState = CursorLockMode.None;

    }
    public void EnforceClose()
    {
        foreach (var slot in enforceSlots)
        {
            slot.ResetSlot();
        }
        Inventory.Instance.ResetAllEquipSlots();

        // 나머지 EnforceClose 로직
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}

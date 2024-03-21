using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightClickButtoninEnforce : MonoBehaviour, IPointerClickHandler
{
    private EnforceSlot slot;
    void Start()
    {
        slot = GetComponent<EnforceSlot>();
        if (slot == null)
        {
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
       
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // 여기서 slot이 null인지 확인해야 합니다.
            // null 체크를 추가해봅시다.
            if (slot != null && slot._c != null)
            {
                Inventory.Instance.AddItemToNextEquipSlot(slot._c);
                // 아이템을 다음 EquipSlot에 추가하는 로직을 여기에 구현합니다.
            }
            else
            {
            }
        }
    }
}

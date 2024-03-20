using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightClickButtoninEnforce : MonoBehaviour, IPointerClickHandler
{
    private InventorySlot slot;
    void Start()
    {
        slot = GetComponentInParent<InventorySlot>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Inventory.Instance.SetSelectedSlot(slot);

            Inventory.Instance.SelectEnforceItem();
        }
    }
}

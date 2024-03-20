using UnityEngine;
using UnityEngine.EventSystems; // EventSystems 네임스페이스를 사용

public class RightClickButton : MonoBehaviour, IPointerClickHandler
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

  
            Inventory.Instance.OnClickRemoveSelectButton();
        }
    }
}
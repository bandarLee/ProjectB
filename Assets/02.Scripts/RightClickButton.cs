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
        // PointerEventData로부터 클릭한 마우스 버튼 확인
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Inventory.Instance.SetSelectedSlot(slot);

            // 우클릭일 경우 실행할 로직
            // 여기에 우클릭 시 실행할 함수나 로직 추가
            Inventory.Instance.OnClickRemoveSelectButton();
        }
    }
}
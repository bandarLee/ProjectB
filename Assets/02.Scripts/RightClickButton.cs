using UnityEngine;
using UnityEngine.EventSystems; // EventSystems 네임스페이스를 사용

public class RightClickButton : MonoBehaviour, IPointerClickHandler
{
    
    public void OnPointerClick(PointerEventData eventData)
    {
        // PointerEventData로부터 클릭한 마우스 버튼 확인
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // 우클릭일 경우 실행할 로직
            Debug.Log("우클릭으로 버튼이 클릭되었습니다.");
            // 여기에 우클릭 시 실행할 함수나 로직 추가
            Inventory.Instance.OnClickRemoveSelectButton();
        }
    }
}
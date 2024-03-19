using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image HoverImage;
    public Image HoverImage2;
    void Start()
    {
        HoverImage.gameObject.SetActive(false);
        HoverImage2.gameObject.SetActive(false);
        
    }

    // 마우스 커서가 버튼 위에 올라갔을 때 호출되는 함수
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 이미지를 활성화합니다.
        HoverImage.gameObject.SetActive(true);
        HoverImage2.gameObject.SetActive(true);
    }

    // 마우스 커서가 버튼에서 벗어났을 때 호출되는 함수
    public void OnPointerExit(PointerEventData eventData)
    {
        // 이미지를 비활성화합니다.
        HoverImage.gameObject.SetActive(false);
        HoverImage2.gameObject.SetActive(false);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class EnforceSlot : MonoBehaviour
{


    public Image ItemImage1;
    public Image ItemImage2;
    public Image ItemImage3;
    public TextMeshProUGUI ItemNameText;

    public ItemData _c;
    public void Start()
    {
        ItemImage1.gameObject.SetActive(false);
        ItemImage2.gameObject.SetActive(false);
        ItemImage3.gameObject.SetActive(false);
    }
    public void UpdateItemImage(ItemData itemData)
    {
        // 아이템 ID 분석 및 적절한 이미지 활성화
        int itemIdDivision = itemData.itemID / 10000;
        switch (itemIdDivision)
        {
            case 1:
                ItemImage1.gameObject.SetActive(true);
                ItemImage2.gameObject.SetActive(false);
                ItemImage3.gameObject.SetActive(false);
                break;
            case 2:
            case 3:
                ItemImage1.gameObject.SetActive(false);
                ItemImage2.gameObject.SetActive(true);
                ItemImage3.gameObject.SetActive(false);
                break;
            case 4:
                ItemImage1.gameObject.SetActive(false);
                ItemImage2.gameObject.SetActive(false);
                ItemImage3.gameObject.SetActive(true);
                break;
            default:
                // 기본 상태 또는 예외 처리
                break;
        }
        _c = itemData;
    }
    public void ResetSlot()
    {
        // ItemData를 null로 설정
        _c = null;

        // 모든 이미지를 비활성화
        ItemImage1.gameObject.SetActive(false);
        ItemImage2.gameObject.SetActive(false);
        ItemImage3.gameObject.SetActive(false);

        // 아이템 이름 텍스트도 초기화
        if (ItemNameText != null)
        {
            ItemNameText.text = "";
        }
    }
}



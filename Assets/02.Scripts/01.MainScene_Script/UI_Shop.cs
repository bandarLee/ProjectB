using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour
{
    public Image ShopImage;
    public TextMeshProUGUI ShopTextUI;

    public Image[] ItemImages;

    private int currentitemIndex;

    private void Awake()
    {
        ShopClose();
        SetItemActive(-1);
    }
    public void ShopOpen() 
    {
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
    public void ShopClose() 
    {
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void OnItemButtonClicked(int itemIndex) 
    {

        currentitemIndex = itemIndex;
        SetItemActive(itemIndex);
        Cursor.lockState = CursorLockMode.None;

    }
    private void SetItemActive(int activeItemIndex) 
    {
        for (int i = 0; i < ItemImages.Length; i++) 
        {
            bool isActive = (i == activeItemIndex);
            ItemImages[i].gameObject.SetActive(isActive);
        }
    }
    public void OnPurchaseClicked()
    {
        switch (currentitemIndex)
        {
            case 0:
                if(PlayerStat.Instance.gold >= 5)
                {
                    PlayerStat.Instance.gold -= 5;
                    PlayerStat.Instance.SmallPotion++;
                }
                ShopClose();
                break;

            case 1:
                if (PlayerStat.Instance.gold >= 20)
                {
                    PlayerStat.Instance.gold -= 20;
                    PlayerStat.Instance.MediumPotion++;
                }
                ShopClose();

                break;
            case 2:
                if (PlayerStat.Instance.gold >= 50)
                {
                    PlayerStat.Instance.gold -= 50;
                    PlayerStat.Instance.LargePotion++;
                }
                ShopClose();

                break;
        }
    }
}

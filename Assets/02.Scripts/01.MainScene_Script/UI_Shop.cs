using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour
{
    public Image ShopImage;
    public TextMeshProUGUI ShopTextUI;

    public Image[] ItemImages;

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
        SetItemActive(itemIndex);
    }
    private void SetItemActive(int activeItemIndex) 
    {
        for (int i = 0; i < ItemImages.Length; i++) 
        {
            bool isActive = (i == activeItemIndex);
            ItemImages[i].gameObject.SetActive(isActive);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour
{
    public Image ShopImage;
    public TextMeshProUGUI ShopTextUI;

    /*public Image ItemAImage;
    public Image ItemBImage;
    public Image ItemCImage;*/

    public Image[] ItemImages;


    private void Awake()
    {
        ShopClose();
        SetItemActive(-1);
        /*ItemAImage.gameObject.SetActive(false);
        ItemBImage.gameObject.SetActive(false);
        ItemCImage.gameObject.SetActive(false);*/
    }
    public void ShopOpen() 
    {
        gameObject.SetActive(true);
    }
    public void ShopClose() 
    {
        gameObject.SetActive(false);
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
    /*public void OnItemA_ButtonClicked() 
    {
        ItemAImage.gameObject.SetActive(true);
        ItemBImage.gameObject.SetActive(false);
        ItemCImage.gameObject.SetActive(false);
    }
    public void OnItemB_ButtonClicked()
    {
        ItemBImage.gameObject.SetActive(true);
        ItemAImage.gameObject.SetActive(false);
        ItemCImage.gameObject.SetActive(false);
    }
    public void OnItemC_ButtonClicked()
    {
        ItemCImage.gameObject.SetActive(true);
        ItemAImage.gameObject.SetActive(false);
        ItemBImage.gameObject.SetActive(false);
    }*/
}

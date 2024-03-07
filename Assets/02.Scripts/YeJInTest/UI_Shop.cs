using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour
{
    public Image ShopImage;
    public TextMeshProUGUI ShopTextUI;



    private void Awake()
    {
        ShopClose();
    }
    public void ShopOpen() 
    {
        gameObject.SetActive(true);
    }
    public void ShopClose() 
    {
        gameObject.SetActive(false);
    }
}

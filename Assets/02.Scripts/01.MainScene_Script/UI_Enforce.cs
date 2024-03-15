using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Enforce : MonoBehaviour
{
    public Image EnforceImage;
    public TextMeshProUGUI EnforceTextUI;



    private void Awake()
    {
        EnforceClose();
    }
    public void EnforceOpen()
    {
        gameObject.SetActive(true);
        PlayerMove.instance.isPositionFixed = true;
    }
    public void EnforceClose()
    {
        gameObject.SetActive(false);
        PlayerMove.instance.isPositionFixed = false;
    }
}

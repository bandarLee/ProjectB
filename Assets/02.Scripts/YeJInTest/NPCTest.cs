using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum NPCType_e
{
    QuestNPC,
    MerchantNPC,
    BlacksmithNPC,
}
public class NPCTest : MonoBehaviour
{
    private Animator _animator;

    public NPCType _NPCType;

    public UI_Mission MissionUI;
    public UI_Shop ShopUI;
    public UI_Enforce EnforceUI;

    public event Action One;

    public GameObject otherObject;

    private bool isOpen = false;

    public GameObject EKeyObject;
    void Start()
    {
        _animator = GetComponent<Animator>();

        One += OneText_Delegate;

        EKeyObject.SetActive(false);
    }
    private void DoorIsTrigger() 
    {
        Collider otherCollider = otherObject.GetComponent<Collider>();
        if (otherObject != null) 
        {
            otherCollider.isTrigger = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("충돌");
        EKeyObject.SetActive(true);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (_NPCType == NPCType.QuestNPC)
                {
                    _animator.SetTrigger("Turn");
                    StartCoroutine(Quest_Coroutine());
                }
                else if (_NPCType == NPCType.MerchantNPC)
                {
                    if (isOpen == true) 
                    {
                        StartCoroutine(ShopCoroutine());
                    }
                    else
                    {
                        StartCoroutine(ShopCloseCoroutine());
                    }

                }
                else if (_NPCType == NPCType.BlacksmithNPC)
                {
                    _animator.SetTrigger("Device");
                    EnforceUI.EnforceOpen();
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            ShopUI.ShopClose();
            EnforceUI.EnforceClose();
        }
        EKeyObject.SetActive(false);
    }

    private IEnumerator Quest_Coroutine()
    {
        MissionUI.Open();
        One?.Invoke();
        yield return new WaitForSeconds(10f);
        MissionUI.Close();
        DoorIsTrigger();
    }
    private void OneText_Delegate()
    {
        MissionUI.FirstMissionOpenText();
        One -= OneText_Delegate;
    }

    private IEnumerator ShopCoroutine() 
    {
        _animator.SetTrigger("Talking");
        ShopUI.ShopOpen();
        yield return new WaitForSeconds(0.2f);
        isOpen = false;
    }
    private IEnumerator ShopCloseCoroutine() 
    {
        ShopUI.ShopClose();
        yield return new WaitForSeconds(0.2f);
        isOpen = true;
    }
}


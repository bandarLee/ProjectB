using System;
using System.Collections;
using System.Collections.Generic;
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

    private bool isOpen = true;

    void Start()
    {
        _animator = GetComponent<Animator>();

        One += OneText_Delegate;
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
                else if (_NPCType == NPCType.MerchantNPC && isOpen)
                {

                    StartCoroutine(ShopCoroutine());
                    
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
        if (isOpen == true)
        {
            _animator.SetTrigger("Talking");
            ShopUI.ShopOpen();
        }
        else 
        {
            yield return new WaitForSeconds(1.0f);


            isOpen = false;
            ShopUI.ShopClose();
        }

        
        
    }
}

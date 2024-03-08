using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum NPCType 
{
    QuestNPC,
    MerchantNPC,
    BlacksmithNPC,
}
public class NPC : MonoBehaviour
{
    private Animator _animator;

    private Transform _targetPlayer;
    private float FindDistance = 5f;
    private float ShopFindDistance = 1f;
    private bool _isTurning = false;
    public NPCType _NPCType;

    public UI_Mission MissionUI;
    public UI_Shop ShopUI;
    public UI_Enforce EnforceUI;

    public event Action One;

    private bool isShopOpen = true;
    private bool isEnforceOpen = true;

   // public GameObject coll;

    void Start()
    {
        _animator = GetComponent<Animator>();

        _targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;

        One += OneText_Delegate;
    }
    private void Update()
    {
        //Debug.Log(Vector3.Distance(_targetPlayer.position, transform.position));
        if (_NPCType == NPCType.QuestNPC && Vector3.Distance(_targetPlayer.position, transform.position) < FindDistance)
        {
            _isTurning = true;
            _animator.SetTrigger("Turn");
            if (Input.GetKey(KeyCode.E)) 
            {
                StartCoroutine(Quest_Coroutine());
            }
        }
        else if (_NPCType == NPCType.QuestNPC && _isTurning && (Vector3.Distance(_targetPlayer.position, transform.position) > FindDistance))
        {
            _animator.SetTrigger("LeftTurn");
            _isTurning = false;
        }

        if (_NPCType == NPCType.MerchantNPC && Vector3.Distance(_targetPlayer.position, transform.position) < ShopFindDistance)
        {
            _animator.SetTrigger("Talking");
            if (Input.GetKeyDown(KeyCode.V))
            {
                if (isShopOpen == true)
                {
                    ShopUI.ShopOpen();
                    isShopOpen = false;
                }
                else
                {
                    ShopUI.ShopClose();
                    isShopOpen = true;
                }

            }
        }
        

        if (_NPCType == NPCType.BlacksmithNPC && Vector3.Distance(_targetPlayer.position, transform.position) < ShopFindDistance)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                if (isEnforceOpen == true)
                {
                    EnforceUI.EnforceOpen();
                    isEnforceOpen = false;
                }
                else
                {
                    EnforceUI.EnforceClose();
                    isEnforceOpen = true;
                }

            }
        }
    }

    
    private IEnumerator Quest_Coroutine() 
    {
        MissionUI.Open();
        One?.Invoke();
        yield return new WaitForSeconds(10f);
        MissionUI.Close();
    }
    private void OneText_Delegate() 
    {
        MissionUI.FirstMissionOpenText();
        One -= OneText_Delegate;
    }
}


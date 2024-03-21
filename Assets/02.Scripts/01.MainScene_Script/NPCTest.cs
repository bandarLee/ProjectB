using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum NPCType
{
    MissionNPC,
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
    public UI_Quest QuestUI;

    // 이벤트
    public event Action One;

    public GameObject EKeyObject;

    public GameObject Scene1PortalLimitedWall;
    public GameObject Scene2PortalLimitedWall;

    private bool isOpen = false;
    private bool isMinimapPoral = false;


    public TextMeshProUGUI NameTextUI;
    

   
    void Start()
    {
        _animator = GetComponent<Animator>();

        // 이벤트에 OneText_Delegate 메서드를 추가
        One += OneText_Delegate;

        EKeyObject.SetActive(false);
        NameTextUI.gameObject.SetActive(false);
    }
    private void DoorIsTrigger() 
    {
        Collider otherCollider = Scene1PortalLimitedWall.GetComponent<Collider>();
        if (Scene1PortalLimitedWall != null) 
        {
            otherCollider.isTrigger = true;
        }
    }
    private void DoorIsTrigger2()
    {
        Collider otherCollider = Scene2PortalLimitedWall.GetComponent<Collider>();
        if (Scene2PortalLimitedWall != null)
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
                NameTextUI.gameObject.SetActive(true);
                if (_NPCType == NPCType.MissionNPC)
                {
                    NameTextUI.text = "MissionNPC";
                    _animator.SetTrigger("Turn");
                    
                    GameManager.instance.StopBlinking();

                    if (PlayerStat.Instance.isPortalArrive != true)
                    {
                        StartCoroutine(Scene1Mission_Coroutine());

                        if (!isMinimapPoral)
                        {
                            GameManager.instance.MinimapPortalStart();
                            isMinimapPoral = true;
                        }
                    }
                    else if (PlayerStat.Instance.isPortalArrive == true) 
                    {
                        StartCoroutine(Scene2Mission_Coroutine());
                        Scene1PortalLimitedWall.GetComponent <Collider>().isTrigger = true;
                    }
                    
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

    // 미션 진행을 나타내는 코루틴
    private IEnumerator Scene1Mission_Coroutine()
    {
        MissionUI.Open();                            // 미션 UI 열기 
        One?.Invoke();                               // One 이벤트를 호출
        yield return new WaitForSeconds(10f);        // 10초간 대기
        MissionUI.Close();                           // 미션 UI 닫기
        DoorIsTrigger();                             // 문이 트리거 되었음을 처리
    }
    private IEnumerator Scene2Mission_Coroutine()
    {
        MissionUI.Open();                            // 미션 UI 열기 
        One?.Invoke();                               // One 이벤트를 호출
        yield return new WaitForSeconds(3f);        // 10초간 대기
        MissionUI.Close();                           // 미션 UI 닫기
        DoorIsTrigger2();                             // 문이 트리거 되었음을 처리
    }
    // One 이벤트 핸들러 - 미션의 첫 번째 메세지를 표시
    private void OneText_Delegate()
    {
        MissionUI.FirstMissionOpenText();             // 미션의 첫 번째 메시지를 표시
        One -= OneText_Delegate;                      // 이벤트 핸들러를 One 이벤트에서 제거

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


using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public UI_Option OptionUI;

    public GameObject MinimapNPC;
    private Coroutine _blinkingCoroutine;
    private Coroutine _blinkingCoroutine2;
    public GameObject Enforce;
    public GameObject MinimapPortal;

    public GameObject[] TutorialObject;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Time.timeScale = 1f;

        MinimapNPC.SetActive(false);
        MinimapPortal.SetActive(false);
        Enforce.SetActive(false);
        if (PlayerStat.Instance.isPortalArrive == true) 
        {
            foreach (GameObject obj in TutorialObject) 
            {
                Destroy(obj);
                PlayerMove.instance.isPositionFixed = false;
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }
    public void Continue()
    {
        Time.timeScale = 1f;
    }
   
    // 미니맵에서 NPC 위치를 깜빡깜빡 거리며 나타냄
    private IEnumerator BlinkingEffect()
    {
        MinimapNPC.SetActive(true);
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            // 활성화 되어 있으면 비활성화로, 비활성화 되어 있으면 활성화로 ( 깜빡이는 효과 )
            MinimapNPC.SetActive(!MinimapNPC.activeSelf);
        }
    }
    public void StopBlinking()
    {
        StopCoroutine(BlinkingEffect());
        Destroy(MinimapNPC);
    }
    public void StartBlinking()
    {
        _blinkingCoroutine = StartCoroutine(BlinkingEffect());
    }

    private IEnumerator MinimapPortalBlinkingEffect()
    {
        yield return new WaitForSeconds(10f);

        MinimapPortal.SetActive(true);
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            // 활성화 되어 있으면 비활성화로, 비활성화 되어 있으면 활성화로 ( 깜빡이는 효과 )
            MinimapPortal.SetActive(!MinimapPortal.activeSelf);
        }
    }
    public void MinimapPortalStart()
    {
        _blinkingCoroutine = StartCoroutine(MinimapPortalBlinkingEffect());
    }
    public void MinimapPortalStop()
    {
        StopCoroutine(MinimapPortalBlinkingEffect());
        Destroy(MinimapPortal);
    }
}

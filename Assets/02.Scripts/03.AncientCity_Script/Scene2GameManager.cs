using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Scene2GameManager : MonoBehaviour
{
    public static Scene2GameManager instance { get; private set; }

    public UI_Option OptionUI;

    public GameObject MinimapNPC;
    public GameObject MinimapNPC2;  // 테스트 해보려고 무기강화NPC 넣어둠
    private Coroutine _blinkingCoroutine;
    private Coroutine _blinkingCoroutine2;

    public GameObject Wall;

    public bool _isCandle1 = false;
    public bool _isCandle2 = false;
    public bool _isCandle3 = false;

    public GameObject RealQuestUI;
    public TextMeshProUGUI RealQuestTextUI;
    string RealQuestText;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Time.timeScale = 1f;

        MinimapNPC.SetActive(false);
        MinimapNPC2.SetActive(false);

        RealQuestUI.SetActive(false);
        StartCoroutine(StartNPCQuestOpen());
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }
    public void Continue()
    {
        Time.timeScale = 1f;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnOptionButtonClicked();
        }

    }
    public void OnOptionButtonClicked()
    {
        Pause();
        OptionUI.Open();
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
    private IEnumerator BlinkingEffect2()
    {
        MinimapNPC2.SetActive(true);
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            // 활성화 되어 있으면 비활성화로, 비활성화 되어 있으면 활성화로 ( 깜빡이는 효과 )
            MinimapNPC2.SetActive(!MinimapNPC2.activeSelf);
        }
    }
    public void StopBlinking()
    {
        StopCoroutine(BlinkingEffect());
        Destroy(MinimapNPC);
    }
    public void StopBlinking2()
    {
        StopCoroutine(BlinkingEffect2());
        Destroy(MinimapNPC2);
    }
    public void StartBlinking()
    {
        _blinkingCoroutine = StartCoroutine(BlinkingEffect());
    }
    public void StartBlinking2()
    {
        _blinkingCoroutine = StartCoroutine(BlinkingEffect2());
    }

    public void OnCandle1()
    {
        _isCandle1 = true;
        CheckWall();
    }
    public void OnCandle2()
    {
        _isCandle2 = true;
        CheckWall();
    }
    public void OnCandle3()
    {
        _isCandle3 = true;
        CheckWall();
        
    }
    private void CheckWall()
    {
        if (_isCandle1 && _isCandle2 && _isCandle3)
        {
            StartCoroutine(DestroyWall());
        }
        
    }
    
    private IEnumerator DestroyWall() 
    {
        yield return new WaitForSeconds(2f);
        Destroy(Wall);
    }

    private IEnumerator Quest(string talk)
    {
        RealQuestTextUI.text = null;
        for (int i = 0; i < talk.Length; i++)
        {
            RealQuestTextUI.text += talk[i];
            yield return new WaitForSeconds(0.08f);
        }
        /*yield return new WaitForSeconds(1f);
        ImageClose();
        yield return new WaitForSeconds(1f);
        Scene2GameManager.instance.StartBlinking();*/

    }


    // NPC 퀘스트
    public void RealQuestImageOpen()
    {
        RealQuestUI.gameObject.SetActive(true);

        PlayerMove.instance.isPositionFixed = true;
    }
    public void RealQuestImageClose()
    {
        RealQuestUI.gameObject.SetActive(false);

        PlayerMove.instance.isPositionFixed = false;
    }
    public void RealQuestTextOpen()
    {
        RealQuestImageOpen();
        PlayerMove.instance.isPositionFixed = false;
        RealQuestText = "NPC1을 찾으세요.";
        StartCoroutine(Quest(RealQuestText));
    }

    public void RealQuestTextOpen2()
    {
        RealQuestImageOpen();
        RealQuestText = "집에 가고싶다.";
        StartCoroutine(Quest(RealQuestText));
    }

    private IEnumerator StartNPCQuestOpen()
    {
        yield return new WaitForSeconds(4f);
        RealQuestTextOpen();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Mission : MonoBehaviour
{
    public static UI_Mission instance { get; private set; }

    public Image ImageUI;
    public TextMeshProUGUI MissionTextUI;
    string text;

    private UI_Quest _QuestUI;

    

    private void Awake()
    {
        instance = this;
        Close();
    }
    public void Open() 
    {
        gameObject.SetActive(true);
        PlayerMove.instance.isPositionFixed = true;
    }
    public void Close()
    {
        gameObject.SetActive(false);
        PlayerMove.instance.isPositionFixed = false;
    }

    public void FirstMissionOpenText() 
    {
        if (PlayerStat.Instance.isPortalArrive != true)
        {
            text = "Zeta-3 행성 .. 생명 반응 감지..\n'첫 번째 임무'\nZeta-3으로 이동하는 포탈을 타고\n생명반응에 대한 조사 명령 하달.";
            StartCoroutine(FirstMission(text));
        }
        else if (PlayerStat.Instance.isPortalArrive == true) 
        {
            text = "ㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋ디짐?\n상점이나 가셈ㅋ";
            StartCoroutine(FirstMission(text));
        }
        
    }
    private IEnumerator FirstMission(string talk) 
    {
        MissionTextUI.text = null;

        for (int i = 0; i < talk.Length; i++) 
        {
            MissionTextUI.text += talk[i];
            PlayerMove.instance.isPositionFixed = true;
            yield return new WaitForSeconds(0.1f);
        }
    }
}

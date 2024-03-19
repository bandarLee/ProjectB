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
        Debug.Log("닫힘");
    }

    public void FirstMissionOpenText() 
    {
        text = "[플레이어]님\nZeta-3 행성에서 생명반응이 보였습니다\n'첫 번째 임무'\nZeta-3으로 이동하는 포탈을 타고 생명반응의 원인을 조사해주세요";
        StartCoroutine(FirstMission(text));
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

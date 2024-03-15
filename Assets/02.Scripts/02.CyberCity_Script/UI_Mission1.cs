using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Mission1 : MonoBehaviour
{
    public static UI_Mission1 instance { get; private set; }

    public Image MissionImageUI;
    public TextMeshProUGUI MissionTextUI;
    string text;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
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
    public void NPC1MissionOpenText()
    {
        Open();
        text = "금요일이다앗..\n졸려워융ㅇ우우...\n현재 오전 11시 4분";
        StartCoroutine(NPCMission(text));
    }
    public void NPC2MissionOpenText()
    {
        Open();
        text = "얼른 끝내고 싶드앙";
        StartCoroutine(NPCMission(text));
    }
    private IEnumerator NPCMission(string talk)
    {
        MissionTextUI.text = null;

        for (int i = 0; i < talk.Length; i++)
        {
            MissionTextUI.text += talk[i];
            PlayerMove.instance.isPositionFixed = true;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1.5f);
        Close();
    }
}

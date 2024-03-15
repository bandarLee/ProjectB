using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Mission2 : MonoBehaviour
{
    public static UI_Mission2 instance { get; private set; }

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
        text = "야호...\n신난다...\n행복해....";
        StartCoroutine(NPCMission(text));
    }
    public void NPC2MissionOpenText() 
    {
        Open();
        text = "집에 가고싶어요...\n왜 아직 목요일일까요....\n그래도 내일은 금요일...";
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

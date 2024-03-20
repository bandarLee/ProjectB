using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Quest2 : MonoBehaviour
{
    public Image QuestImage;
    public TextMeshProUGUI QuestTextUI;
    string text;


    private void Start()
    {
        QuestTextOpen();
    }
    public void QuestTextOpen() 
    {
        ImageOpen();
        text = "Chapter 2.\nAncient City";
        StartCoroutine(Quest(text));
    }
    private IEnumerator Quest(string talk)
    {
        QuestTextUI.text = null;
        for (int i = 0; i < talk.Length; i++)
        {
            QuestTextUI.text += talk[i];
            yield return new WaitForSeconds(0.08f);
            AncientCitySceneAudioManager.instance.PlayAudio(1); //쳅터텍스트음향
        }
        yield return new WaitForSeconds(1f);
        ImageClose();
        yield return new WaitForSeconds(1f);
        Scene2GameManager.instance.StartBlinking();

    }
    public void ImageOpen()
    {
        QuestImage.gameObject.SetActive(true);

        PlayerMove.instance.isPositionFixed = true;
    }
    public void ImageClose()
    {
        QuestImage.gameObject.SetActive(false);

        PlayerMove.instance.isPositionFixed = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Quest : MonoBehaviour
{
    public Image QuestImage;
    public TextMeshProUGUI QuestTextUI;
    string text;


    private void Start()
    {
        ImageOpen();
        text = "커맨트 센터로\n이동하세요";
        StartCoroutine(Quest(text));
        
    }
    private IEnumerator Quest(string talk) 
    {
        QuestTextUI.text = null;

        for (int i = 0; i < talk.Length; i++) 
        {
            QuestTextUI.text += talk[i];
            yield return new WaitForSeconds(0.08f);
        }
        yield return new WaitForSeconds(1f);
        ImageClose();
    }
    public void ImageOpen() 
    {
        QuestImage.gameObject.SetActive(true);
    }
    public void ImageClose() 
    {
        QuestImage.gameObject.SetActive(false); 
    }
}

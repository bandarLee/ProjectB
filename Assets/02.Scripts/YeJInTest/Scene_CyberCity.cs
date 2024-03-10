using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scene_CyberCity : MonoBehaviour
{
    public Image Image;
    public TextMeshProUGUI textMeshProUGUI;
    private string text;

    private void Start()
    {
        text = "Scene 1 . Cyber City";
        StartCoroutine(FirstMission(text));
    }
    private IEnumerator FirstMission(string talk)
    {
        textMeshProUGUI.text = null;

        for (int i = 0; i < talk.Length; i++)
        {
            textMeshProUGUI.text += talk[i];
            yield return new WaitForSeconds(0.1f);
        }
    }
}

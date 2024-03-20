using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyScene : MonoBehaviour
{
    public TextMeshProUGUI TextUI;
    string text;

    public GameObject StartButton;
    public GameObject EndButton;

    private void Start()
    {
        text = "PROJECT_B";
        StartCoroutine(LobbyText(text));
        StartButton.SetActive(false);
        EndButton.SetActive(false);
    }
    private void Update()
    {

    }
    
    private IEnumerator LobbyText(string talk)
    {
        TextUI.text = null;

        for (int i = 0; i < talk.Length; i++)
        {
            TextUI.text += talk[i];
            yield return new WaitForSeconds(0.1f);
        }

        StartButton.SetActive(true);
        EndButton.SetActive(true);
    }

    public void OnClickStartButton() 
    {
        SceneManager.LoadScene("Main");
        LobbySceneAudioManager.instance.PlayAudio(4);// 클릭시 이동음
    }
    public void OnExitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

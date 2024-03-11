using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Option : MonoBehaviour
{
    public void Open() 
    {
        gameObject.SetActive(true);
    }
    public void Close() 
    {
        gameObject.SetActive(false);
    }
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    public void OnContinueButtonClicked() 
    {
        Close();
        GameManager.instance.Continue();
    }
    public void OnExitButtonClicked() 
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

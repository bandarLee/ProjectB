using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Option : MonoBehaviour
{
    public void Open() 
    {
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
    public void Close() 
    {
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
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

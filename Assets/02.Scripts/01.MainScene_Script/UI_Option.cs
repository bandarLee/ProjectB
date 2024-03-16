using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class UI_Option : MonoBehaviour
{
    public GameObject[] UIs;
    private static UI_Option m_instance;

    public static UI_Option Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UI_Option>();
            }

            return m_instance;
        }
    }
    public void Open() 
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
    public void Close() 
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Awake()
    {
    }
    private void Start()
    {

    }
    public void OnContinueButtonClicked() 
    {
        Close();
/*     GameManager.instance.Continue();
*/    }
    public void OnExitButtonClicked() 
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

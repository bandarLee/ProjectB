using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_GameOver : MonoBehaviour
{
    public GameObject GameOverUI;

    private void Start()
    {
        GameOverUI.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            GameOverUI.gameObject.SetActive(true);
        }
    }

    public void OnClickGameOverButton() 
    {
        PlayerStat.Instance.isPortalArrive = true;
        SceneManager.LoadScene("Main");
    }
}

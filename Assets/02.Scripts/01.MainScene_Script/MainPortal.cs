using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum MainPortalType 
{
    CyberCity,
    AncientCity
}
public class MainPortal : MonoBehaviour
{
    public MainPortalType PortalType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && PortalType == MainPortalType.CyberCity) 
        {
            StartCoroutine(NextScene());
        }
        else if (other.gameObject.CompareTag("Player") && PortalType == MainPortalType.AncientCity)
        {
            StartCoroutine(NextScene2());
        }
    }
    private IEnumerator NextScene() 
    {
        yield return new WaitForSeconds(0.5f);
        PlayerStat.Instance.isPortalArrive = true;
        SceneManager.LoadScene("TestScene");
    }
    private IEnumerator NextScene2() 
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Scene2");
    }
}

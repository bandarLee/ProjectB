using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum PortalType 
{
    CyberCity,
    AncientCity
}
public class Portal : MonoBehaviour
{
    public PortalType PortalType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && PortalType == PortalType.CyberCity) 
        {
            StartCoroutine(NextScene());
        }
        else if (other.gameObject.CompareTag("Player") && PortalType == PortalType.AncientCity)
        {
            StartCoroutine(NextScene2());
        }
    }
    private IEnumerator NextScene() 
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("TestScene");
    }
    private IEnumerator NextScene2() 
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Scene2");
    }
}

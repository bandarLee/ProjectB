using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AncientCityPortal : MonoBehaviour
{

    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            StartCoroutine(NextScene());
        }
    }
    private IEnumerator NextScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("LobbyScene");
    }
}

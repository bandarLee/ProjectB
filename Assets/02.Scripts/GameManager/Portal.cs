using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            StartCoroutine(NextScene());
        }
    }
    private IEnumerator NextScene() 
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("TestScene");
    }

}

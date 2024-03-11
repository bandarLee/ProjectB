using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(MainScene());
    }
    private IEnumerator MainScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Main");
    }
}

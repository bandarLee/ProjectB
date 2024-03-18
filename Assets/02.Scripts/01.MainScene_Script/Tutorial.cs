using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject Tutorial_Key;

    private void Start()
    {
        Tutorial_Key.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        Tutorial_Key.gameObject.SetActive(true);
        StartCoroutine(TutorialTextCoroutine());
    }

    private IEnumerator TutorialTextCoroutine() 
    {
        yield return new WaitForSeconds(2f);
        Destroy(Tutorial_Key);
    }

}

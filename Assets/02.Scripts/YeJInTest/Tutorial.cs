using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public TextMeshProUGUI TutorialTextUI;

    public Image TutorialImageUI;

    private void Start()
    {
        TutorialImageUI.gameObject.SetActive(false);
        TutorialTextUI.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        TutorialImageUI.gameObject.SetActive(true);
        TutorialTextUI.gameObject.SetActive(true);
        StartCoroutine(TutorialTextCoroutine());
        
    }

    private IEnumerator TutorialTextCoroutine() 
    {
        yield return new WaitForSeconds(2f);
        Destroy(TutorialImageUI);
        Destroy(TutorialTextUI);
    }

}

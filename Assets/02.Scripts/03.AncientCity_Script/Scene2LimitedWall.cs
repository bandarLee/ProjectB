using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene2LimitedWall : MonoBehaviour
{
    public Image LimitedWallUI;

    private void Start()
    {
        LimitedWallUI.gameObject.SetActive(false);
    }
   
   
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            LimitedWallUI.gameObject.SetActive(true);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LimitedWallUI.gameObject.SetActive(false);
        }
    }
}

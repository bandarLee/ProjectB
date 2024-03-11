using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }
    public void Pause() 
    {
        Time.timeScale = 0f;
    }
    public void Continue() 
    {
        Time.timeScale = 1f;
    }
}

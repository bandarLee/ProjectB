using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public UI_Option OptionUI;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void Pause() 
    {
        Time.timeScale = 0f;
    }
    public void Continue() 
    {
        Time.timeScale = 1f;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) 
        {
            OnOptionButtonClicked();
        }
        
    }
    public void OnOptionButtonClicked() 
    {
        Pause();
        OptionUI.Open();
    }
}

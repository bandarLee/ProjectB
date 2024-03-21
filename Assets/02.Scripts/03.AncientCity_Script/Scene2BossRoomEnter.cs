using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CandleLightType 
{
    Candle1,
    Candle2,
    Candle3
}
public class Scene2BossRoomEnter : MonoBehaviour
{
    public GameObject EKey;
    public CandleLightType CandleType;

   

    private void Start()
    {
        EKey.gameObject.SetActive(false);
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            EKey.gameObject.SetActive(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (CandleType == CandleLightType.Candle1)
                {
                    Scene2GameManager.instance.OnCandle1();
                    
                }
                else if (CandleType == CandleLightType.Candle2)
                {
                    Scene2GameManager.instance.OnCandle2();
                    
                }
                else if (CandleType == CandleLightType.Candle3)
                {
                    Scene2GameManager.instance.OnCandle3();
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            EKey.gameObject.SetActive(false);
        }
    }
   
}

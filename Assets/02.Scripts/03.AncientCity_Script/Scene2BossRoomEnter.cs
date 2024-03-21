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
    public GameObject CandleVFX1;
   

    private void Start()
    {
        EKey.gameObject.SetActive(false);

        CandleVFX1.gameObject.SetActive(false);
        
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
                    CandleVFX1.gameObject.SetActive(true);
                }
                else if (CandleType == CandleLightType.Candle2)
                {
                    Scene2GameManager.instance.OnCandle2();
                    CandleVFX1.gameObject.SetActive(true);
                }
                else if (CandleType == CandleLightType.Candle3)
                {
                    Scene2GameManager.instance.OnCandle3();
                    CandleVFX1.gameObject.SetActive(true);
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

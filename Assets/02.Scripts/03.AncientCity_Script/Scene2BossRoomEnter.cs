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

    public GameObject Wall1;
    public GameObject Wall2;

    public bool _isCandle1 = false;
    public bool _isCandle2 = false;
    public bool _isCandle3 = false;

    private void Start()
    {
        EKey.gameObject.SetActive(false);
        Wall1.gameObject.SetActive(false);
        Wall2.gameObject.SetActive(false);
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
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (CandleType == CandleLightType.Candle1)
                {
                    Debug.Log("1번 켜짐");
                    _isCandle1 = true;
                    CheckWall();
                    Debug.Log(_isCandle1);
                    Debug.Log(_isCandle2);
                    Debug.Log(_isCandle3);
                }
                else if (CandleType == CandleLightType.Candle2)
                {
                    Debug.Log("2번 켜짐");
                    _isCandle2 = true;
                    CheckWall();
                    Debug.Log(_isCandle1);
                    Debug.Log(_isCandle2);
                    Debug.Log(_isCandle3);
                }
                else if (CandleType == CandleLightType.Candle3)
                {
                    Debug.Log("3번 켜짐");
                    _isCandle3 = true;
                    CheckWall();
                    Debug.Log(_isCandle1);
                    Debug.Log(_isCandle2);
                    Debug.Log(_isCandle3);
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
    private void CheckWall() 
    {
        if (_isCandle1 == true && _isCandle2 == true && _isCandle3 == true) 
        {
            Wall1.gameObject.SetActive(true);
            Wall2.gameObject.SetActive(true);
        }
    }
}

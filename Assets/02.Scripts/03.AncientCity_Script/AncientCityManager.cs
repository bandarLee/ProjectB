using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientCityManager : MonoBehaviour
{
    private static MainManager _instance;
    public GameObject PlayerPrefab;
    public static MainManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MainManager>();
            }

            return _instance;
        }
    }
    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
        Vector3 Playerposition = new Vector3(263.899f, 15.77f, 284.755f);
        Quaternion rotation = Quaternion.Euler(new Vector3(0, -180f, 0));

        PlayerMove.instance.gameObject.transform.position = Playerposition;
        PlayerMove.instance.gameObject.transform.rotation = rotation;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    private static MainManager m_instance;
    public GameObject PlayerPrefab;
    public static MainManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<MainManager>();
            }

            return m_instance;
        }
    }
    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
        Vector3 Playerposition = new Vector3(-5.01f, 3.174f, -58.404f);
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        PlayerMove.instance.gameObject.transform.position = Playerposition;
        PlayerMove.instance.gameObject.transform.rotation = rotation;

    }
}

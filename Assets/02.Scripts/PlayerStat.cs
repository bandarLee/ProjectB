using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class PlayerStat : MonoBehaviour
{
    private static PlayerStat m_instance;
    public float playermaxhealth;
    public float playerhealth;
    public float str;
    public float dronestr;
    public float speed;

    public static PlayerStat instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<PlayerStat>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }
    void Awake()
    {
        if (instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
      
            DontDestroyOnLoad(gameObject);
       

    }
    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("MonsterBullet"))
        {

            playerhealth -= 1;

            if (playerhealth <= 0)
            {
                Debug.LogWarning("플레이어사망");
            }

            Destroy(collision.gameObject);
        }
    }
    void Update()
    {

    }
}

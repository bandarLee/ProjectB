using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using UnityEngine.SceneManagement;

public class PlayerStat : MonoBehaviour
{
    private static PlayerStat m_instance;
    public int playerhealth;
    public int str;
    public int dex;
    public int speed;

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
    }

    void Update()
    {
        
    }
}

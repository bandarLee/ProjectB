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
    public float Timer = 5f;
    private float frozenTimerBox;




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
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("MonsterBullet"))
        {

            playerhealth -= 1;

            if (playerhealth <= 0)
            {
                Debug.LogWarning("플레이어사망");
            }

            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("MonsterElementBullet"))
        {
            CyberpunkMonsterBullet cyberpunkmonsterBulletScript = other.gameObject.GetComponent<CyberpunkMonsterBullet>();
            CyberpunkMonsterBulletType cyberpunkMonsterBulletType = cyberpunkmonsterBulletScript.cyberpunkMonsterBulletType;


            switch (cyberpunkMonsterBulletType)
            {
                case CyberpunkMonsterBulletType.Health:
                {
                    // 체력 -1 
                    playerhealth -= 1;
                }
                break;

                case CyberpunkMonsterBulletType.GuidedMissile:
                {
                    
                        Timer -= Time.deltaTime;
                        playerhealth -= 2;
                        PlayerMove.instance.isJumping = true;
                    PlayerMove.instance.isFlying = true;
                     
                    // 5초간 점프, 비행 불가
                }
                break;

                case CyberpunkMonsterBulletType.Smoke:
                {
                    playerhealth -= 1;
                }
                break;

                case CyberpunkMonsterBulletType.Boom:
                {
                   
                        Timer -= Time.deltaTime;
                        playerhealth -= 3;
                        PlayerMove.instance.isJumping = true;
                        PlayerMove.instance.isFlying = true;
                        if (Timer <= 0)
                        {
                        PlayerMove.instance.isJumping = false;
                        PlayerMove.instance.isFlying = false;
                    }
                    
                    Timer = 5f;
                }
                break;

                case CyberpunkMonsterBulletType.Frozen:
                {
                    Timer = 3f;
                    Timer -= Time.deltaTime;
                    playerhealth -= 1;
                    frozenTimerBox = speed;
                    speed = 0;
                    if (Timer <= 0)
                    {
                        speed = frozenTimerBox;
                    }
                    Timer = 5;
                }
                break;
                
            }

            if (playerhealth <= 0)
            {
                Debug.LogWarning("플레이어사망");
            }

            Destroy(other.gameObject);
        }

        else if (other.gameObject.CompareTag("MonsterElementBullet"))
        {
            TempleMonsterBullet templeMonsterBulletScript = other.gameObject.GetComponent<TempleMonsterBullet>();
            TempleBulletType templeBulletType = templeMonsterBulletScript.templeBulletType;


            switch(templeBulletType)
            {
                case TempleBulletType.Health:
                {
                    // 체력 -1 
                    playerhealth -= 1;
                }
                break;
                //작성중
            }


            if (playerhealth <= 0)
            {
                Debug.LogWarning("플레이어사망");
            }

            Destroy(other.gameObject);
        }
    }
    void Update()
    {

    }
}

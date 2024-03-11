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
        else if (collision.gameObject.CompareTag("MonsterElementBullet"))
        {
            CyberpunkMonsterBullet cyberpunkmonsterBulletScript = collision.gameObject.GetComponent<CyberpunkMonsterBullet>();
            CyberpunkMonsterBulletType cyberpunkMonsterBulletType = cyberpunkmonsterBulletScript.cyberpunkMonsterBulletType;

            PlayerMove playerMove = collision.gameObject.GetComponent<PlayerMove>();

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
                    if(playerMove != null)
                    {
                        Timer -= Time.deltaTime;
                        playerhealth -= 2;
                        playerMove.IsJumping = true;
                        playerMove.IsFlying = true;
                        if (Timer <= 0)
                        {
                            playerMove.IsJumping = false;
                            playerMove.IsFlying = false;
                        }
                    }
                    // 5초간 점프, 비행 불가
                    Timer = 5f;
                }
                break;

                case CyberpunkMonsterBulletType.Smoke:
                {
                    playerhealth -= 1;
                }
                break;

                case CyberpunkMonsterBulletType.Boom:
                {
                    if (playerMove != null)
                    {
                        Timer -= Time.deltaTime;
                        playerhealth -= 3;
                        playerMove.IsJumping = true;
                        playerMove.IsFlying = true;
                        if (Timer <= 0)
                        {
                            playerMove.IsJumping = false;
                            playerMove.IsFlying = false;
                        }
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

            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.CompareTag("MonsterElementBullet"))
        {
            TempleMonsterBullet templeMonsterBulletScript = collision.gameObject.GetComponent<TempleMonsterBullet>();
            TempleBulletType templeBulletType = templeMonsterBulletScript.templeBulletType;

            PlayerMove playerMove = collision.gameObject.GetComponent<PlayerMove>();

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

            Destroy(collision.gameObject);
        }
    }
    void Update()
    {

    }
}

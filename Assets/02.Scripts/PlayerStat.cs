using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;


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
    public float forceAmount = -500f;
    private Rigidbody playerRigidbody;
    public Volume volume;
    private ColorAdjustments colorAdjustments;
    public Slider healthBarSlider;
    public Slider healthBarSlider_Option;


    public bool isInvulnerable = false;
    public float invulnerabilityDuration = 1f;
    public static PlayerStat instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<PlayerStat>();
            }

            return m_instance;
        }
    }
    void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
            DontDestroyOnLoad(gameObject);

            playerRigidbody = GetComponent<Rigidbody>();
       

    }
    private void Start()
    {
        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            colorAdjustments.saturation.Override(0);
        }
        InitializeHealthBar();

    }

    void OnTriggerEnter(Collider other)
    {
       


        if (!isInvulnerable)
        {
            isInvulnerable = true;
            StartCoroutine(Waitfor1second());

            if (other.gameObject.CompareTag("MonsterBullet"))
            {

                playerhealth -= 1;
                UpdateHealthBar();

                TakeDamage();

                Vector3 forceDirection = transform.position - other.transform.position;
                forceDirection.y = 0;
                forceDirection.Normalize();

                playerRigidbody.AddForce(forceDirection * forceAmount);
                StartCoroutine(ChangeTimeScale(1));


                if (playerhealth <= 0)
                {
                    Debug.LogWarning("플레이어사망");
                }

                Destroy(other.gameObject);
            }
            if (other.gameObject.CompareTag("Boss1Bullet"))
            {

                playerhealth -= 1;
                UpdateHealthBar();

                TakeDamage();

                Vector3 forceDirection = transform.position - other.transform.position;
                forceDirection.y = 0;
                forceDirection.Normalize();

                playerRigidbody.AddForce(forceDirection * forceAmount);
                StartCoroutine(ChangeTimeScale(1));

                if (playerhealth <= 0)
                {
                    Debug.LogWarning("플레이어사망");
                }

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
                        UpdateHealthBar();

                    }
                    break;

                    /*  case CyberpunkMonsterBulletType.GuidedMissile:
                      {

                              Timer -= Time.deltaTime;
                              playerhealth -= 2;
                          UpdateHealthBar();

                          PlayerMove.instance.isJumping = true;
                          PlayerMove.instance.isFlying = true;

                          // 5초간 점프, 비행 불가
                      }
                      break;*/

                    case CyberpunkMonsterBulletType.Smoke:
                    {
                        playerhealth -= 1;
                        UpdateHealthBar();

                    }
                    break;

                    case CyberpunkMonsterBulletType.Boom:
                    {

                        Timer -= Time.deltaTime;
                        playerhealth -= 3;
                        UpdateHealthBar();

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
                        UpdateHealthBar();

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


                switch (templeBulletType)
                {
                    case TempleBulletType.Health:
                    {
                        // 체력 -1 
                        playerhealth -= 1;
                        UpdateHealthBar();

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
            IEnumerator ChangeTimeScale(float delay)
            {
                Time.timeScale = 0.2f;

                yield return new WaitForSecondsRealtime(delay);

                Time.timeScale = 1f;
                yield return new WaitForSeconds(1);

                isInvulnerable = false;



            }
        }
    }
    public void TakeDamage()
    {
        if (colorAdjustments != null)
        {
            colorAdjustments.saturation.Override(-100);
        }

        Invoke("ResetColorAdjustments", 1f);
    }
    IEnumerator Waitfor1second()
    {

        yield return new WaitForSeconds(1);
        isInvulnerable = false;

    }

    void ResetColorAdjustments()
    {
        if (colorAdjustments != null)
        {
            colorAdjustments.saturation.Override(0);
        }
    }
    private void InitializeHealthBar()
    {
        healthBarSlider.maxValue = playermaxhealth;
        healthBarSlider.value = playerhealth;
        healthBarSlider_Option.maxValue = playermaxhealth;
        healthBarSlider_Option.value = playerhealth;

    }
    public void UpdateHealthBar()
    {
        healthBarSlider.value = playerhealth;
        healthBarSlider_Option.value = playerhealth;

    }
    void Update()
    {

    }
}

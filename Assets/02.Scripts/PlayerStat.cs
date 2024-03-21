using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class PlayerStat : MonoBehaviour
{
    private static PlayerStat m_instance;

    public float level = 1;
    public float playermaxhealth = 10;
    public float playerhealth = 10;

    public float exp = 0;
    public float maxexp = 100;

    public float gold = 0;


    public float str = 1;
    public float dronestr = 0.5f;
    public float dronedex = 0.5f;
    public float speed;


    public float Timer = 5f;
    private float frozenTimerBox;
    public float forceAmount = -500f;
    private Rigidbody playerRigidbody;
    public Volume volume;
    private ColorAdjustments colorAdjustments;
    public Slider healthBarSlider;
    public Slider healthBarSlider_Option;
    public GameObject GameOverUI;


    public bool isInvulnerable = false;
    public float invulnerabilityDuration = 1f;
    public bool isdead = false;

    public TextMeshProUGUI[] StatusText;
    public TextMeshProUGUI[] EquipStatusText;

    public bool isPortalArrive = false;
    public Dictionary<int, StatChangeLog> statChangeLogs = new Dictionary<int, StatChangeLog>();

    public int SmallPotion = 0;
    public int MediumPotion = 0;
    public int LargePotion = 0;

    public TextMeshPro PotionNotice;



    public static PlayerStat Instance
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
        if (Instance != this)
        {
            Destroy(gameObject);
        }
            DontDestroyOnLoad(gameObject);

            playerRigidbody = GetComponent<Rigidbody>();
       

    }
    private void Start()
    {
        GameOverUI.gameObject.SetActive(false);
        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            colorAdjustments.saturation.Override(0);
        }
        

        InitializeHealthBar();
        InitializeOptionUpdate();

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


                
                Destroy(other.gameObject);
            }
            if (other.gameObject.CompareTag("Boss1Bullet"))
            {

                playerhealth -= 1;
                UpdateHealthBar();
                PlayerAudioManager.instance.PlayAudio(8);// 데미지 플레이어 소리
                TakeDamage();

                Vector3 forceDirection = transform.position - other.transform.position;
                forceDirection.y = 0;
                forceDirection.Normalize();

                playerRigidbody.AddForce(forceDirection * forceAmount);
                StartCoroutine(ChangeTimeScale(1));

                
            }
            if (other.gameObject.CompareTag("Boss2Bullet"))
            {
                playerhealth -= 2;
                UpdateHealthBar();

                TakeDamage();

                Vector3 forceDirection = transform.position - other.transform.position;
                forceDirection.y = 0;
                forceDirection.Normalize();

                playerRigidbody.AddForce(forceDirection * forceAmount);
                StartCoroutine(ChangeTimeScale(1));

                
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
    public void InitializeOptionUpdate()
    {
        StatusText[0].text = $"LV : {level}";
        StatusText[1].text = $"{playerhealth}/{playermaxhealth}";
        StatusText[2].text = $"{exp}/{maxexp}";
        StatusText[3].text = $"{gold}G";

        StatusText[4].text = $"{10 * str}";
        StatusText[5].text = $"{10 * str *1.5f}";
        StatusText[6].text = $"{10 * dronestr}";
        StatusText[7].text = $"{10 * dronedex}";
        StatusText[8].text = $"{10 * speed}";

        float totalStrChange = 0;
        float totalDexChange = 0;
        float totalDmgChange = 0;
        float totalSpeedChange = 0;

        foreach (var log in statChangeLogs.Values)
        {
            totalStrChange += log.strChange;
            totalDexChange += log.dexChange;
            totalDmgChange += log.dmgChange;
            totalSpeedChange += log.speedChange;
        }

        EquipStatusText[0].text = $"(+{10 * totalStrChange})";
        EquipStatusText[1].text = $"(+{10 * totalStrChange * 1.5f})";

        EquipStatusText[2].text = $"(+{10 * totalDexChange})";
        EquipStatusText[3].text = $"(+{10 * totalDmgChange})";
        EquipStatusText[4].text = $"(+{10 * totalSpeedChange})";




    }
    void Update()
    {

        if (playerhealth <= 0 && !isdead)
        {
            isdead = true;
            PlayerAudioManager.instance.PlayAudio(9);// 사망 사운드
            PlayerAudioManager.instance.PlayAudio(10);// 사망 후 배경음 // 시간수정필요 혹은 사망UI에 적용

            StartCoroutine(GameOver_Coroutine());

            PlayerAudioManager.instance.PlayAudio(8);// 데미지 사운드

        }
        UsePotion();
    }
    public IEnumerator GameOver_Coroutine() 
    {
        GameOverUI.gameObject.SetActive(true);
        isPortalArrive = true;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Main");
        playerhealth = playermaxhealth;
        GameOverUI.gameObject.SetActive(false);
        isdead = false;

    }
    public void ApplyStatChange(int itemId, StatChangeLog change)
    {
        StatChangeLog roundedChange = new StatChangeLog
        {
            strChange = (float)Math.Round(change.strChange, 4),
            dexChange = (float)Math.Round(change.dexChange, 4),
            dmgChange = (float)Math.Round(change.dmgChange, 4),
            speedChange = (float)Math.Round(change.speedChange, 4),
        };

        str = (float)Math.Round(str + roundedChange.strChange, 4);
        dronedex = (float)Math.Round(dronedex + roundedChange.dexChange, 4);
        speed = (float)Math.Round(speed + roundedChange.speedChange, 4);
        dronestr = (float)Math.Round(dronestr + roundedChange.dmgChange, 4);

        statChangeLogs[itemId] = roundedChange;

        InitializeOptionUpdate();
    }

    public void RemoveStatChange(int itemId)
    {
        if (statChangeLogs.TryGetValue(itemId, out StatChangeLog change))
        {
            str = (float)Math.Round(str - change.strChange, 4);
            dronedex = (float)Math.Round(dronedex - change.dexChange, 4);
            speed = (float)Math.Round(speed - change.speedChange, 4);
            dronestr = (float)Math.Round(dronestr - change.dmgChange, 4);

            statChangeLogs.Remove(itemId);

            InitializeOptionUpdate();
        }
    }
    [System.Serializable]
    public struct StatChangeLog
    {
        public float strChange;
        public float dexChange;
        public float speedChange;
        public float dmgChange;

    }
    public void UsePotion()
    {
        PotionNotice.text = $"{SmallPotion} / {MediumPotion} / {LargePotion}";

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (SmallPotion > 0)
            {
                SmallPotion--;
                playerhealth += 1;
                CheckMaxHealth();

                UpdateHealthBar();

            }

        }


        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (MediumPotion > 0)
            {
                MediumPotion--;
                playerhealth += 2;
                CheckMaxHealth();

                UpdateHealthBar();

            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (LargePotion > 0)
            {
                LargePotion--;
                playerhealth += 3;
                CheckMaxHealth();
                UpdateHealthBar();


            }
        }
    }
    public void CheckMaxHealth()
    {
        if(playerhealth > playermaxhealth)
        {
            playerhealth = playermaxhealth;
        }
    }


}

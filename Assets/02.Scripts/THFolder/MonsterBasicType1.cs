using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterBasicType1 : MonoBehaviour
{
    public float health = 2;
    public float attackDelay = 0.6f;
    private float lastAttackTime = 0f;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    private float attackRange = 20f;
    private bool isShooting = false;
    private float attackGracePeriod = 2f;
    private float lastTimePlayerInRange;
    GameObject player;
    public Slider healthBarSlider;
    public GameObject healthBarUI;
    public TextMeshProUGUI damage;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerHead");
        healthBarUI.SetActive(false);
        InitializeHealthBar();


    }
    void Update()

    {        
        firePoint.rotation = transform.rotation;

        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < attackRange)
        {
            lastTimePlayerInRange = Time.time;
        }
    


        if (Time.time - lastTimePlayerInRange < attackGracePeriod && !isShooting)
        {
            StartCoroutine(ShootAtPlayerCoroutine());
        }
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
        UpdateHealthBar();
    }
    private void OnTriggerEnter(Collider other)
    {
        {
            float damageAmount = 0;

            if (other.gameObject.CompareTag("basicweapon") && Time.time >= lastAttackTime + attackDelay)
            {
                healthBarUI.SetActive(true);
                Debug.Log("기본무기맞음");
                damageAmount = PlayerStat.instance.str;

                health -= damageAmount;
                StartCoroutine(ShowDamageCoroutine(damageAmount));

                lastAttackTime = Time.time;
            }
            if (other.gameObject.CompareTag("bullet")) 
            {
                healthBarUI.SetActive(true);
                PlayerBullet playerbullet = other.gameObject.GetComponent<PlayerBullet>();
                if(playerbullet.playerbullettype == PlayerBullet.PlayerBulletType.DroneBullet)
                {
                    damageAmount = PlayerStat.instance.dronestr;
                    health -= damageAmount;
                    StartCoroutine(ShowDamageCoroutine(damageAmount));


                }
                else if (playerbullet.playerbullettype == PlayerBullet.PlayerBulletType.StrongBullet)
                {
                    damageAmount = PlayerStat.instance.dronestr * 3;
                    health -= damageAmount;
                    StartCoroutine(ShowDamageCoroutine(damageAmount));


                }

            }
        }
    }
    IEnumerator ShootAtPlayerCoroutine()
    {
        isShooting = true;
        Vector3 direction = (player.transform.position - firePoint.position).normalized;

        GameObject monsterbullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = monsterbullet.GetComponent<Rigidbody>();

        rb.velocity = direction * 15f;

        monsterbullet.transform.rotation = Quaternion.LookRotation(direction);
                
            

        yield return new WaitForSeconds(fireRate);
        isShooting = false;


    }
    void InitializeHealthBar()
    {
        healthBarSlider.maxValue = health;
        healthBarSlider.value = health;
    }
    void UpdateHealthBar()
    {
        healthBarSlider.value = health;
    }
    IEnumerator ShowDamageCoroutine(float damageAmount)
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < 30)
        {
            damage.fontSize = 36; // 기본 크기
        }
        
        else if (distance < 40)
        {
            damage.fontSize = 45; 
        }
        else if (distance < 50)
        {
            damage.fontSize = 54; 
        }
        else if (distance < 60)
        {
            damage.fontSize = 63;
        }
        else
        {
            damage.fontSize = 72; 
        }
        damage.text = $"{damageAmount}";
        damage.gameObject.SetActive(true); 

        yield return new WaitForSeconds(1);

        damage.gameObject.SetActive(false);
    }
}

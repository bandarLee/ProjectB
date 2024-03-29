using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scene2MonsterBasic : MonoBehaviour
{

    public float health = 3;
    public float attackDelay = 1f;
    private float lastAttackTime = 0f;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    private float attackRange = 20f;
    private bool isShooting = true;
    private float attackGracePeriod = 2f;
    private float lastTimePlayerInRange;
    GameObject player;
    public Slider healthBarSlider;
    public GameObject healthBarUI;
    public TextMeshProUGUI damage;
    public Animator templemonsteranimator;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerHead");
        healthBarUI.SetActive(false);
        InitializeHealthBar();
        templemonsteranimator = GetComponent<Animator>();

        StartCoroutine(ShootAtPlayerAfterDelay(2));


    }
    void Update()

    {
        RotateTowardsPlayer();

        firePoint.rotation = transform.rotation;

        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < attackRange)
        {
            lastTimePlayerInRange = Time.time;

        }
        else if (distance > attackRange) {
            templemonsteranimator.SetBool("IsAttack", false);

        }


        if (Time.time - lastTimePlayerInRange < attackGracePeriod && !isShooting)
        {
                templemonsteranimator.SetBool("IsAttack", true);

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
                damageAmount = PlayerStat.Instance.str;

                health -= damageAmount;
                StartCoroutine(ShowDamageCoroutine(damageAmount));

                lastAttackTime = Time.time;
            }
            if (other.gameObject.CompareTag("bullet"))
            {
                healthBarUI.SetActive(true);
                PlayerBullet playerbullet = other.gameObject.GetComponent<PlayerBullet>();
                if (playerbullet.playerbullettype == PlayerBullet.PlayerBulletType.DroneBullet)
                {
                    damageAmount = PlayerStat.Instance.dronestr;
                    health -= damageAmount;
                    StartCoroutine(ShowDamageCoroutine(damageAmount));


                }
                else if (playerbullet.playerbullettype == PlayerBullet.PlayerBulletType.StrongBullet)
                {
                    damageAmount = PlayerStat.Instance.dronestr * 3;
                    health -= damageAmount;
                    StartCoroutine(ShowDamageCoroutine(damageAmount));


                }

            }
        }
    }
    private IEnumerator ShootAtPlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isShooting = false;

    }
    IEnumerator ShootAtPlayerCoroutine()
    {
        isShooting = true;
        Vector3 direction = (player.transform.position - firePoint.position).normalized;

        GameObject monsterbullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = monsterbullet.GetComponent<Rigidbody>();

        rb.velocity = direction * 10f;

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
    void RotateTowardsPlayer()
    {
        if (player == null) return;

        Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

        Vector3 direction = targetPosition - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

       

        float rotationSpeed = 2f;
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

}

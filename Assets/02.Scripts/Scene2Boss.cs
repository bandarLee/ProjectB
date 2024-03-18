using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using static Scene1Boss;

public class Scene2Boss : MonoBehaviour
{
    public float health = 200;

    public float attackDelay = 0f;
    private float lastAttackTime = 0f;
    public Slider healthBarSlider;
    public GameObject healthBarUI;
    public TextMeshProUGUI damage;
    GameObject player;

    private Animator animator;
    public enum Boss2Pattern
    {
        Walk,
        Attack1,
        Attack2,
        Attack3,
        Attack4,
        Attack5,
    }
    public Boss2Pattern pattern = Boss2Pattern.Walk;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerHead");

        healthBarUI.SetActive(false);
        InitializeHealthBar();
        animator = GetComponent<Animator>();
        StartCoroutine(PatternCoroutine());

    }

    void Update()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
        UpdateHealthBar();
        RotateTowardsPlayer();


    }
    IEnumerator PatternCoroutine()
    {
        while (health > 0)
        {

            switch (pattern)
            {
                case Boss2Pattern.Walk:
                    animator.SetInteger("PatternIndex", (int)pattern);

                    yield return new WaitForSeconds(5.05f);

                    pattern = DetermineNextAttackPattern();
                    break;

                case Boss2Pattern.Attack1:
                    animator.SetInteger("PatternIndex", (int)pattern);
                    yield return new WaitForSeconds(5.05f);

                    pattern = Boss2Pattern.Walk;
                    break;

                case Boss2Pattern.Attack2:
                    animator.SetInteger("PatternIndex", (int)pattern);
                    yield return new WaitForSeconds(5.05f);
                    pattern = Boss2Pattern.Walk;
                    break;



                case Boss2Pattern.Attack3:
                    animator.SetInteger("PatternIndex", (int)pattern);

                    yield return new WaitForSeconds(5.05f);

                    pattern = Boss2Pattern.Walk;
                    break;

                case Boss2Pattern.Attack4:
                    animator.SetInteger("PatternIndex", (int)pattern);

                    yield return new WaitForSeconds(5.05f);
                    pattern = Boss2Pattern.Walk;
                    break;

                case Boss2Pattern.Attack5:
                    animator.SetInteger("PatternIndex", (int)pattern);
                    yield return new WaitForSeconds(5.05f);

                    pattern = Boss2Pattern.Walk;
                    break;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        {
            float damageAmount = 0;

            if (other.gameObject.CompareTag("basicweapon") && Time.time >= lastAttackTime + attackDelay)
            {
                healthBarUI.SetActive(true);
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
                    damageAmount = PlayerStat.Instance.str * 1.5f;
                    health -= damageAmount;
                    StartCoroutine(ShowDamageCoroutine(damageAmount));

                    damage.text = $"{PlayerStat.Instance.str * 1.5}";

                }

            }
        }
    }
    Boss2Pattern DetermineNextAttackPattern()
    {
        int nextPatternIndex = UnityEngine.Random.Range(1, 6);


        return (Boss2Pattern)nextPatternIndex;
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
    void InitializeHealthBar()
    {
        healthBarSlider.maxValue = health;
        healthBarSlider.value = health;
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

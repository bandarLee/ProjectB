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
    public GameObject Attack1Thunder;
    public Quaternion additionalRotation = Quaternion.Euler(0, 0, 0);
    public GameObject[] cubewallsPrefab;
    public GameObject[] Attack5wallsPrefab;


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
        Attack1Thunder.SetActive(false);
        foreach (GameObject A5wall in Attack5wallsPrefab)
        {
            A5wall.SetActive(false);
        }
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

                    yield return new WaitForSeconds(3f);

                    pattern = DetermineNextAttackPattern();
                    break;

                case Boss2Pattern.Attack1:
                    animator.SetInteger("PatternIndex", (int)pattern);
                    Attack1Thunder.SetActive(true);

                    yield return new WaitForSeconds(4.025f);
                    Attack1Thunder.SetActive(false);

                    pattern = Boss2Pattern.Walk;
                    break;

                case Boss2Pattern.Attack2:
                    animator.SetInteger("PatternIndex", (int)pattern);
                    StartCoroutine(Attack2());

                    yield return new WaitForSeconds(5.05f);
                    pattern = Boss2Pattern.Walk;
                    break;



                case Boss2Pattern.Attack3:
                    animator.SetInteger("PatternIndex", (int)pattern);

                    yield return new WaitForSeconds(4.367f);

                    pattern = Boss2Pattern.Walk;
                    break;

                case Boss2Pattern.Attack4:
                    animator.SetInteger("PatternIndex", (int)pattern);
                    additionalRotation = Quaternion.Euler(0, 70, 0);
                    yield return new WaitForSeconds(4.0925f);
                    pattern = Boss2Pattern.Walk;
                    additionalRotation = Quaternion.Euler(0, 0, 0);

                    break;

                case Boss2Pattern.Attack5:
                    animator.SetInteger("PatternIndex", (int)pattern);
                    additionalRotation = Quaternion.Euler(0, 55, 0);
                    yield return new WaitForSeconds(3f);

                    foreach (GameObject A5wall in Attack5wallsPrefab)
                    {
                        A5wall.SetActive(true);
                    }
                    yield return new WaitForSeconds(1.0925f);
                    foreach (GameObject A5wall in Attack5wallsPrefab)
                    {
                        A5wall.SetActive(false);
                    }
                    pattern = Boss2Pattern.Walk;
                    additionalRotation = Quaternion.Euler(0, 0, 0);

                    break;
            }
        }
    }
    public IEnumerator Attack2()
    {
        // 초기 위치와 회전 상태를 저장
        Vector3[] initialPositions = new Vector3[cubewallsPrefab.Length];
        Quaternion[] initialRotations = new Quaternion[cubewallsPrefab.Length];
        for (int i = 0; i < cubewallsPrefab.Length; i++)
        {
            initialPositions[i] = cubewallsPrefab[i].transform.position;
            initialRotations[i] = cubewallsPrefab[i].transform.rotation;
        }

        // 4방향 정의
        Vector3[] directions = new Vector3[]
        {
        Vector3.back,
        Vector3.right,
        Vector3.forward,
        Vector3.left,
        };
        yield return new WaitForSeconds(1.3f);

        foreach (GameObject cube in cubewallsPrefab)
        {
            cube.SetActive(true);
        }
        yield return new WaitForSeconds(2.3f);

        for (int i = 0; i < cubewallsPrefab.Length; i++)
        {
            Rigidbody cubeRb = cubewallsPrefab[i].GetComponent<Rigidbody>();
            if (cubeRb != null)
            {
                cubeRb.velocity = Vector3.zero;
                cubeRb.AddForce(directions[(i / 3) % directions.Length] * 10, ForceMode.Impulse);
            }
        }

        yield return new WaitForSeconds(3.8f);

        for (int i = 0; i < cubewallsPrefab.Length; i++)
        {
            cubewallsPrefab[i].transform.position = initialPositions[i];
            cubewallsPrefab[i].transform.rotation = initialRotations[i];
            cubewallsPrefab[i].SetActive(false);
            Rigidbody cubeRb = cubewallsPrefab[i].GetComponent<Rigidbody>();
            if (cubeRb != null)
            {
                cubeRb.velocity = Vector3.zero;
                cubeRb.angularVelocity = Vector3.zero;
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

        Quaternion finalRotation = lookRotation * additionalRotation;

        float rotationSpeed = 2f; 
        transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, rotationSpeed * Time.deltaTime);
    }


}

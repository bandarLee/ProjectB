using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class Scene1Boss : MonoBehaviour
{
    public float health = 200;

    public float attackDelay = 0f;
    private float lastAttackTime = 0f;
    public Slider healthBarSlider;
    public GameObject healthBarUI;
    public TextMeshProUGUI damage;
    GameObject player;
    private Animator animator;
    public GameObject mechPrefab;

    public GameObject cubePrefab;
    public GameObject[] cubewallsPrefab;

    public GameObject bossHead;
    private float explosionForce = 2000f;
    private float explosionRadius = 3f;
    private GameObject cubeInstance;

    private GameObject cubewheeInstance;
    public GameObject cubewheeIPrefab;
    public GameObject cubewheeIPrefab2;

    public GameObject starfield;


    public enum Boss1Pattern
    {
        Walk,
        Attack1,
        Attack2,
        Attack3,
        Attack4,
        Attack5,
    }
    public Boss1Pattern pattern = Boss1Pattern.Walk;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerHead");
        healthBarUI.SetActive(false);
        InitializeHealthBar();
        animator = GetComponent<Animator>();
        starfield.SetActive(false);
        StartCoroutine(PatternCoroutine());


    }

    void Update()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
        UpdateHealthBar();

    }


    IEnumerator PatternCoroutine()
    {
        while (health > 0)
        {

            switch (pattern)
            {
                case Boss1Pattern.Walk:
                    animator.SetInteger("PatternIndex", (int)pattern);

                    yield return new WaitForSeconds(2.333f);
                    starfield.SetActive(false);
                    yield return new WaitForSeconds(0.667f);

                    pattern = DetermineNextAttackPattern();
                    break;

                case Boss1Pattern.Attack1:
                    animator.SetInteger("PatternIndex", (int)pattern);

                    yield return new WaitForSeconds(1f);
                    Vector3 firstPositionOffset = new Vector3(-6, 0, 7.2f);
                    Instantiate(mechPrefab, transform.position + firstPositionOffset, Quaternion.identity);

                    yield return new WaitForSeconds(1f);
                    Vector3 secondPositionOffset = new Vector3(-1, 0, 7.2f);
                    Instantiate(mechPrefab, transform.position + secondPositionOffset, Quaternion.identity);

                    yield return new WaitForSeconds(1f);
                    Vector3 thirdPositionOffset = new Vector3(4, 0, 7.2f);
                    Instantiate(mechPrefab, transform.position + thirdPositionOffset, Quaternion.identity);

                    yield return new WaitForSeconds(0.833f);
                    Vector3 forthPositionOffset = new Vector3(9, 0, 7.2f);
                    Instantiate(mechPrefab, transform.position + forthPositionOffset, Quaternion.identity);

                    pattern = Boss1Pattern.Walk;
                    break;

                case Boss1Pattern.Attack2:
                    animator.SetInteger("PatternIndex", (int)pattern);
                    StartCoroutine(Attack2());
                    yield return new WaitForSeconds(5.05f);
                    pattern = Boss1Pattern.Walk;
                    break;



                case Boss1Pattern.Attack3:
                    animator.SetInteger("PatternIndex", (int)pattern);
                    StartCoroutine(Attack3());

                    yield return new WaitForSeconds(5.066f);

                    pattern = Boss1Pattern.Walk;
                    break;

                case Boss1Pattern.Attack4:
                    animator.SetInteger("PatternIndex", (int)pattern);
                    StartCoroutine(Attack4());

                    yield return new WaitForSeconds(4.09f);
                    pattern = Boss1Pattern.Walk;
                    break;

                case Boss1Pattern.Attack5:
                    animator.SetInteger("PatternIndex", (int)pattern);
                    yield return new WaitForSeconds(1.3f);
                    starfield.SetActive(true);

                    yield return new WaitForSeconds(1.667f);
                    pattern = Boss1Pattern.Walk;
                    break;
            }
        }
    }
    public IEnumerator Attack2()
    {
        cubeInstance = Instantiate(cubePrefab, bossHead.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(3f);
        Scene1BossCube.boom = true;    

        ExplodeCube();


    }
    public IEnumerator Attack3()
    {
        Vector3[] initialPositions = new Vector3[cubewallsPrefab.Length];
        Quaternion[] initialRotations = new Quaternion[cubewallsPrefab.Length];
        for (int i = 0; i < cubewallsPrefab.Length; i++)
        {
            initialPositions[i] = cubewallsPrefab[i].transform.position;
            initialRotations[i] = cubewallsPrefab[i].transform.rotation;
        }
        Vector3[] directions = new Vector3[]
        {
        Vector3.back,  
        Vector3.right,    
        Vector3.forward,     
        Vector3.left,   
        Vector3.up,      
        };
        foreach (GameObject cube in cubewallsPrefab)
        {
            cube.SetActive(true);
            yield return new WaitForSeconds(0.4f);
        }
        for (int i = 0; i < cubewallsPrefab.Length; i++)
        {
            Rigidbody cubeRb = cubewallsPrefab[i].GetComponent<Rigidbody>();
            if (cubeRb != null)
            {
                cubeRb.velocity = Vector3.zero;
                cubeRb.AddForce(directions[i % directions.Length] * 10, ForceMode.Impulse);
            }
        }

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < cubewallsPrefab.Length; i++)
        {
            cubewallsPrefab[i].transform.position = initialPositions[i];
            cubewallsPrefab[i].transform.rotation = initialRotations[i];
            cubewallsPrefab[i].SetActive(false);
            Rigidbody cubeRb = cubewallsPrefab[i].GetComponent<Rigidbody>();
            cubeRb.velocity = Vector3.zero; 
            cubeRb.angularVelocity = Vector3.zero;

        }
    }
    public IEnumerator Attack4()
    {
        cubewheeInstance = Instantiate(cubewheeIPrefab, bossHead.transform.position + 5 * Vector3.up, Quaternion.identity);
        


        yield return new WaitForSeconds(3f);
        Destroy(cubewheeInstance);
        Instantiate(cubewheeIPrefab2, bossHead.transform.position + 5 * Vector3.up, Quaternion.identity);


    }
    Boss1Pattern DetermineNextAttackPattern()
    {
        int nextPatternIndex = UnityEngine.Random.Range(1, 6);


        return (Boss1Pattern)nextPatternIndex;
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
                    damageAmount = PlayerStat.Instance.str * 1.5f;
                    health -= damageAmount;
                    StartCoroutine(ShowDamageCoroutine(damageAmount));

                    damage.text = $"{PlayerStat.Instance.str * 1.5}";

                }

            }
        }
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
    IEnumerator Waitforsecond(float time)
    {
        yield return new WaitForSeconds(time);
    }


    private void ExplodeCube()
    {
        foreach (Transform child in cubeInstance.transform)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>(); 
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, cubeInstance.transform.position, explosionRadius);
            }
        }

        Destroy(cubeInstance, 5f);
    }
}
    

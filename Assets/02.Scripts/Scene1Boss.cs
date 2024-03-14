using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scene1Boss : MonoBehaviour
{
    public float health = 200;
    public Transform Cubepoint;
    public float attackDelay = 0f;
    private float lastAttackTime = 0f;
    public Slider healthBarSlider;
    public GameObject healthBarUI;
    public TextMeshProUGUI damage;
    GameObject player;

    public enum Boss1Pattern
    {
        Walk,
        Attack1,
        Attack2,
        Attack3,
        Attack4,
        Attack5,
        Attack6
    }
    public Boss1Pattern pattern = Boss1Pattern.Walk;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerHead");
        healthBarUI.SetActive(false);
        InitializeHealthBar();

    }

    void Update()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
        switch (pattern)
        {
            case Boss1Pattern.Walk:
                StartCoroutine(Waitforsecond());
                break;

            case Boss1Pattern.Attack1:
                StartCoroutine(Waitforsecond());

                break;

            case Boss1Pattern.Attack2:
                StartCoroutine(Waitforsecond());

                break;

            case Boss1Pattern.Attack3:
                StartCoroutine(Waitforsecond());

                break;

            case Boss1Pattern.Attack4:
                StartCoroutine(Waitforsecond());

                break;

            case Boss1Pattern.Attack5:
                StartCoroutine(Waitforsecond());

                break;
            case Boss1Pattern.Attack6:
                StartCoroutine(Waitforsecond());

                break;
        }
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
                if (playerbullet.playerbullettype == PlayerBullet.PlayerBulletType.DroneBullet)
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

                    damage.text = $"{PlayerStat.instance.dronestr * 3}";

                }

            }
        }
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
    IEnumerator Waitforsecond()
    {
            yield return new WaitForSeconds(3f);
    }
}

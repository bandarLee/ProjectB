using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerBullet;

public class MonsterBasicType1 : MonoBehaviour
{
    public float health = 2;
    public float attackDelay = 1.15f;
    private float lastAttackTime = 0f;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    private float attackRange = 20f;
    private bool isShooting = false;
    private float attackGracePeriod = 2f;
    private float lastTimePlayerInRange;
    GameObject player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerHead");


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

    }
    private void OnTriggerEnter(Collider other)
    {
        {
            if (other.gameObject.CompareTag("basicweapon") && Time.time >= lastAttackTime + attackDelay)
            {
                Debug.Log("기본무기맞음");
                health = health - PlayerStat.instance.str;
                lastAttackTime = Time.time;

            }
            if (other.gameObject.CompareTag("bullet")) 
            {
                PlayerBullet playerbullet = other.gameObject.GetComponent<PlayerBullet>();
                if(playerbullet.playerbullettype == PlayerBulletType.DroneBullet)
                {
                    health = health - PlayerStat.instance.dronestr;

                }
                else if (playerbullet.playerbullettype == PlayerBulletType.StrongBullet)
                {
                    health = health - (PlayerStat.instance.dronestr*3);

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
}

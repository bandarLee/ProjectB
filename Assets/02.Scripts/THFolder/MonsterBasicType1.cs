using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBasicType1 : MonoBehaviour
{
    public int health = 3;
    public float attackDelay = 1.15f;
    private float lastAttackTime = 0f;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    private float attackRange = 30f;
    private bool isShooting = false;
    private float attackGracePeriod = 1.0f;
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
            if (other.gameObject.tag == "basicweapon" && Time.time >= lastAttackTime + attackDelay)
            {
                health = health - 2;
                lastAttackTime = Time.time;

            }
            if (other.gameObject.tag == "bullet")
            {
                health--;

            }
        }
    }
    IEnumerator ShootAtPlayerCoroutine()
    {
        isShooting = true;
        Vector3 direction = (player.transform.position - firePoint.position).normalized;

        GameObject monsterbullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = monsterbullet.GetComponent<Rigidbody>();

        rb.velocity = direction * 20f;

        monsterbullet.transform.rotation = Quaternion.LookRotation(direction);
                
            

        yield return new WaitForSeconds(fireRate);
        isShooting = false;


    }
}

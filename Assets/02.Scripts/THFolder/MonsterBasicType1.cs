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

    private void Start()
    {
        StartCoroutine(ShootAtPlayerCoroutine());

    }
    void Update()
    {
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
        while (true) 
        {
            GameObject player = GameObject.FindGameObjectWithTag("PlayerHead");
            if (player != null)
            {
                Vector3 direction = (player.transform.position - firePoint.position).normalized;

                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                Rigidbody rb = bullet.GetComponent<Rigidbody>();

                rb.velocity = direction * 20f; 

                bullet.transform.rotation = Quaternion.LookRotation(direction);
            }

            yield return new WaitForSeconds(fireRate);
        }
    }
}

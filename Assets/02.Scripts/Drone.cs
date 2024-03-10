using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Drone : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private bool isCoolingDown = false; 

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !isCoolingDown)
        {
            StartCoroutine(ShootWithCooldown());
        }
    }

    IEnumerator ShootWithCooldown()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        isCoolingDown = true;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * 20f;

        yield return new WaitForSeconds(fireRate);

        isCoolingDown = false;
    }
}

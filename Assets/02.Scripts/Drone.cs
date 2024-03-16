using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Drone : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate; 
    private bool isCoolingDown = false;
    public Transform player;

    public Transform fireDronPoint;
    public Transform firstDronPoint;


    void Update()
    {
        this.gameObject.transform.rotation = player.rotation;

        if (Input.GetKey(KeyCode.LeftShift) && !isCoolingDown)
        {
            StartCoroutine(ShootWithCooldown());
        }
    }
    private void Start()
    {
        fireRate = PlayerStat.instance.dronedex;
    }
    IEnumerator ShootWithCooldown()
    {
        this.gameObject.transform.position = fireDronPoint.position;

        Quaternion additionalRotation = Quaternion.Euler(0, 0, 0);
        Quaternion finalRotation = firePoint.rotation * additionalRotation;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, finalRotation);
        isCoolingDown = true;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = finalRotation * Vector3.forward * 20f;

        yield return new WaitForSeconds(fireRate);

        isCoolingDown = false;
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            this.gameObject.transform.position = firstDronPoint.position;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 20f;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        Destroy(gameObject, 20f);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Environment") || other.CompareTag("enemy"))
        {
            Destroy(gameObject);
        }
    }
}

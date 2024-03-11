using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBasicType1Bullet1 : MonoBehaviour
{
    public float speed = 20f; 
    public float lifetime = 20f; 
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed; 

        Destroy(gameObject, lifetime); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }
}

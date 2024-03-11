using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonsterHealthTh : MonoBehaviour
{
    public int health = 3;
    public float attackDelay = 1.15f;
    private float lastAttackTime = 0f;

    void Update()
    {
        if( health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        {
            if (other.gameObject.tag == "basicweapon" && Time.time >= lastAttackTime + attackDelay)
            {
                health--;
                lastAttackTime = Time.time;

            }
            if (other.gameObject.tag == "bullet")
            {
                health--;

            }
        }
    }


}

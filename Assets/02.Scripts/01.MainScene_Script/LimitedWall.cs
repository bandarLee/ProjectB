using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedWall : MonoBehaviour
{
    public GameObject Wall;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            Destroy(Wall);
        }
    }
}

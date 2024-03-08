using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBulletFire : MonoBehaviour
{
    public GameObject Bullet;
    public Transform Muzzle;


    // Update is called once per frame
    void Update()
    {
        Instantiate(Bullet, Muzzle.transform.position, Muzzle.transform.rotation);
    }
}

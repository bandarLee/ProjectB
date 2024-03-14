using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    //public float bulletSpeed;
    public float BulletSpin = 500f;

    void Update()
    {
        // 총알 앞으로 이동
        //transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward * BulletSpin * Time.deltaTime);
    }
}


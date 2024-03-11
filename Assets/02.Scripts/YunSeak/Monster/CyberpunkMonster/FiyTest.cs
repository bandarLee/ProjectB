using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FiyTest : MonoBehaviour
{
    public float rotationSpeed = 500f;

    private void Start()
    {

    }
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }

}

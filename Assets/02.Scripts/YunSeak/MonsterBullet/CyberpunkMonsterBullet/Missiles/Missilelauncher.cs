using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missilelauncher : MonoBehaviour
{
    [SerializeField] GameObject m_goMissile = null;
    [SerializeField] Transform m_tfMissileSpawn = null;
    public float UpSpeed = 5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input .GetKeyDown(KeyCode.Space))
        {
            GameObject t_missile = Instantiate(m_goMissile, m_tfMissileSpawn.position, Quaternion.identity);
            t_missile.GetComponent<Rigidbody>().velocity = Vector3.up * UpSpeed;
        }
    }
}

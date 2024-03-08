using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Health,  // 체력을 줄인다
    Stamina, // 스피드를 줄인다.
    Smoke   // 연막탄
}

public class MonsterBullet : MonoBehaviour
{
    public BulletType BulletType;
    private Animator _animator;
    private Rigidbody _rigidbody;
    public float _smoketimeer = 8f;

    private void Update()
    {
        transform.Translate(Vector3.forward * 1f);
        if(BulletType == BulletType.Health)
        {
            Health();
        }
        else if(BulletType == BulletType.Stamina)
        {
            Stamina();
        }
        else
        {
            Smoke();
        }



        
    }

    private void Health()
    {
        //플레이어의 체력을 X값 줄이고
        Debug.Log("체력총알");
        Destroy(this.gameObject);

    }

    private void Stamina()
    {
        //플레이어의 스테미너을를 X값 줄이고
        Debug.Log("스테미너 총알");
        Destroy(this.gameObject);
    }

    private void Smoke() 
    {
        _smoketimeer -= Time.deltaTime;
        if(_smoketimeer <= 0)
        {
            Debug.Log("연막탄");
            Destroy(this.gameObject);
        }
    }
    private void OnColliderEnter(Collider collider)
    {

    }
}


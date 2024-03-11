using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CyberpunkMonsterBulletType
{
    Health,  // 체력을 줄인다
    GuidedMissile, // 스피드를 줄인다.
    Smoke,   // 연막탄
    Boom,    // 작렬탄
    Frozen     // 얼음
}

public class CyberpunkMonsterBullet : MonoBehaviour
{
    public CyberpunkMonsterBulletType cyberpunkMonsterBulletType;
    private Animator _animator;
    private Rigidbody _rigidbody;
    public float _smoketimeer = 8f;
    public int Damage = 1;

    public GameObject HealthEffectPrefab;
    public GameObject StaminaEffectPrefab;
    public GameObject SmokeEffectPrefab;
    public GameObject BoomEffectPrefab;
    public GameObject FrozenEffectPrefab;

    private void Update()
    {
        transform.Translate(Vector3.forward * 1f);

        if(cyberpunkMonsterBulletType == CyberpunkMonsterBulletType.Health)
        {
            Health();
        }
        else if (cyberpunkMonsterBulletType == CyberpunkMonsterBulletType.GuidedMissile)
        {
            GuidedMissile();
        }

        else if (cyberpunkMonsterBulletType == CyberpunkMonsterBulletType.Frozen)
        {
            Stop();
        }

        else if (cyberpunkMonsterBulletType == CyberpunkMonsterBulletType.Smoke)
        {
            Smoke();
        }

        else if (cyberpunkMonsterBulletType == CyberpunkMonsterBulletType.Boom)
        {
            Boom(); 
        }       
    }

    private void Health()
    {
        //플레이어의 체력을 X값 줄이고
        Debug.Log("체력총알");
        Destroy(this.gameObject);

    }

    private void GuidedMissile()
    {
        //플레이어의 스테미너을를 X값 줄이고
        Debug.Log("유도탄");
        Destroy(this.gameObject);
    }

    private void Smoke() 
    {
        _animator = GetComponent<Animator>();
        _smoketimeer -= Time.deltaTime;
        if(_smoketimeer <= 0)
        {
            Debug.Log("연막탄");
            Destroy(this.gameObject);
        }
    }
    private void Boom()
    {
        _animator = GetComponent<Animator>();
        _smoketimeer -= Time.deltaTime;
        if (_smoketimeer <= 0)
        {
            Debug.Log("작렬탄");
            Destroy(this.gameObject);
        }
    }

    private void Stop()
    {
        _smoketimeer -= Time.deltaTime;

        Destroy(this.gameObject);
        if (_smoketimeer <= 0)
        {
            Debug.Log("얼음");
            // 플레이어의 스피드를 기존 값으로 복원
        }
    }
    private void OnColliderEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player" && collider.gameObject.tag == "Monster")
        {
             // 체력을 줄인다
        }
        else if(collider.gameObject.tag == "Wall" && collider.gameObject.tag == "Ground")
        {
            Debug.Log("총알 삭제");
            Destroy(this.gameObject);
        }
    }
}


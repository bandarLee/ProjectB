using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CyberpunkMonsterBulletType
{
    Health,        // 체력
    GuidedMissile, // 유도탄
    Smoke,         // 연막탄
    Boom,          // 작렬탄
    Frozen         // 얼음
}

public class CyberpunkMonsterBullet : MonoBehaviour
{
    private Transform _target;
    public float AttackDistance = 2f;  

    public CyberpunkMonsterBulletType cyberpunkMonsterBulletType;
    public NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private Rigidbody _rigidbody;
    public float _smoketimeer = 8f;
    public int Damage = 1;
    public float rotationSpeed = 50f; 

    public GameObject HealthEffectPrefab;
    public GameObject GuidedMissileEffectPrefab;
    public GameObject SmokeEffectPrefab;
    public GameObject BoomEffectPrefab;
    public GameObject FrozenEffectPrefab;


    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Player 또는 Monster에 충돌했을 때
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Monster"))
        {
            // 플레이어 또는 몬스터에게 데미지를 줄 수 있는 인터페이스를 가지고 있는지 확인
            IHitable hitable = collision.gameObject.GetComponent<IHitable>();
            if (hitable != null)
            {
                // 데미지를 줄이고 폭발 이펙트 생성
                hitable.Hit(new DamageInfo(DamageType.Normal, Damage));
                CreateExplosionEffect();
            }
        }
        // Wall 또는 Ground에 충돌했을 때
        else if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground"))
        {
            // 폭발 이펙트 생성
            CreateExplosionEffect();
        }

        // 총알 오브젝트 파괴
        Destroy();
    }


    void CreateExplosionEffect()
    {
        // 타입에 따른 폭발 이펙트 생성
        GameObject explosionEffectPrefab = GetExplosionEffectPrefab();
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }
    }

    public GameObject GetExplosionEffectPrefab()
    {
        switch (cyberpunkMonsterBulletType)
        {
            case CyberpunkMonsterBulletType.Health:
                return HealthEffectPrefab;

            case CyberpunkMonsterBulletType.GuidedMissile:
                return GuidedMissileEffectPrefab;

            case CyberpunkMonsterBulletType.Smoke:
                return SmokeEffectPrefab;

            case CyberpunkMonsterBulletType.Boom:
                return BoomEffectPrefab;

            case CyberpunkMonsterBulletType.Frozen:
                return FrozenEffectPrefab;

            default:
                return null;
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * 1f);

        if (cyberpunkMonsterBulletType == CyberpunkMonsterBulletType.Health)
        {
            Health();
        }
        else if (cyberpunkMonsterBulletType == CyberpunkMonsterBulletType.GuidedMissile)
        {
            GuidedMissile();
        }

        else if (cyberpunkMonsterBulletType == CyberpunkMonsterBulletType.Frozen)
        {
            Frozen();
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
        Debug.Log("체력총알");
    }

    private void GuidedMissile()
    {
        Follow();
        Debug.Log("유도탄");
    }

    private void Smoke()
    {

        _animator = GetComponent<Animator>();
        _smoketimeer -= Time.deltaTime;
        if (_smoketimeer <= 0)
        {
            Debug.Log("연막탄");

        }
    }
    private void Boom()
    {

        _animator = GetComponent<Animator>();
        _smoketimeer -= Time.deltaTime;
        if (_smoketimeer <= 0)
        {
            Debug.Log("작렬탄");
            Destroy();
        }
    }

    private void Frozen()
    {

        _smoketimeer -= Time.deltaTime;

        Destroy(this.gameObject);
        if (_smoketimeer <= 0)
        {
            Debug.Log("얼음");
            Destroy();
        }
    }

    public void Follow()
    {

        Vector3 dir = _target.transform.position - this.transform.position;
        dir.y = 0;
        dir.Normalize();

        _navMeshAgent.stoppingDistance = AttackDistance;

        // 내비게이션의 목적지를 플레이어의 위치로 한다.
        _navMeshAgent.destination = _target.position;
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

}


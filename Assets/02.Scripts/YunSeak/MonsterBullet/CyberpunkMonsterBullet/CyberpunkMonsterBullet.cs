using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CyberpunkMonsterBulletType
{
    Health,        // 체력
    Smoke,         // 연막탄
    Boom,          // 작렬탄
    Frozen         // 얼음
}

public class CyberpunkMonsterBullet : MonoBehaviour
{
    public CyberpunkMonsterBulletType cyberpunkMonsterBulletType;
    private Rigidbody _rigidbody;
    public float _smoketimeer = 8f;
    public int Damage = 1;
    public float rotationSpeed = 50f;
    public GameObject explosionPrefab;  // 폭발 이펙트 프리팹
    [SerializeField] ParticleSystem BoomEffect = null;

    public GameObject HealthEffectPrefab;
    public GameObject GuidedMissileEffectPrefab;
    public GameObject SmokeEffectPrefab;
    public GameObject BoomEffectPrefab;
    public GameObject FrozenEffectPrefab;

    private void OnTriggerEnter(Collider other)
    {
        PlayerStat playerStat = other.GetComponent<PlayerStat>();


        // Player 또는 Monster에 충돌했을 때
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Monster"))
        {
            if (playerStat.playerhealth >= 0)
            {
                Destroy();
                Debug.Log($"플레이어 피격");
                //playerStat.playerhealth -= ;
            }
        }
        // Wall 또는 Ground에 충돌했을 때
        else if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Ground"))
        {
            Destroy();
        }
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

    private void Smoke()
    {
        _smoketimeer -= Time.deltaTime;
        if (_smoketimeer <= 0)
        {
            Debug.Log("연막탄");

        }
    }
    private void Boom()
    {
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

    public void Destroy()
    {
        // 1. 폭발 이펙트 생성
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        // 2. BoomEffect 재생
        BoomEffect.Play();

        Destroy(this.gameObject);
    }
}


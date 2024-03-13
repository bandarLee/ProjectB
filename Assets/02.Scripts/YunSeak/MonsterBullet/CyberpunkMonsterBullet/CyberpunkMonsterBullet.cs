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
    public float _smoketimeer = 8f;
    public int Damage = 1;
    public float rotationSpeed = 50f;
    public float MainFrozenTimer = 3f;
    public float FrozenTimer = 3f;

    [SerializeField] ParticleSystem HealthEffect;
    [SerializeField] ParticleSystem SmokeEffect;
    [SerializeField] ParticleSystem BoomEffect;
    [SerializeField] ParticleSystem FrozenEffect;

    public GameObject HealthEffectPrefab;
    public GameObject SmokeEffectPrefab;
    public GameObject BoomEffectPrefab;
    public GameObject FrozenEffectPrefab;

    public float healthDamageToPlayer = 1f;
    public float smokeDamageToPlayer = 1f;
    public float boomDamageToPlayer = 3f;
    public float frozenDamageToPlayer = 1f;

    private float PlayerSpeedBox;

    private void OnTriggerEnter(Collider other)
    {
        PlayerStat playerStat = other.GetComponent<PlayerStat>();

        if (other.CompareTag("Player"))
        {
            // Player에 충돌했을 때
            switch (cyberpunkMonsterBulletType)
            {
                case CyberpunkMonsterBulletType.Health:

                    HealthDestroy();
                    Debug.Log($"플레이어{healthDamageToPlayer}피격");
                    playerStat.playerhealth -= healthDamageToPlayer;
                    break;

                case CyberpunkMonsterBulletType.Smoke:

                    SmokeDestroy();
                    Debug.Log($"플레이어{smokeDamageToPlayer}피격");
                    playerStat.playerhealth -= smokeDamageToPlayer;
                    break;

                case CyberpunkMonsterBulletType.Boom:

                    BoomDestroy();
                    Debug.Log($"플레이어{boomDamageToPlayer}피격");
                    playerStat.playerhealth -= boomDamageToPlayer;
                    break;

                case CyberpunkMonsterBulletType.Frozen:
                    FrozenDestroy();
                    FrozenTimer -= Time.deltaTime;
                    Debug.Log($"플레이어{frozenDamageToPlayer}피격");
                    playerStat.playerhealth -= frozenDamageToPlayer;
                    PlayerSpeedBox = playerStat.speed ;
                    playerStat.speed = 0f;
                    if(FrozenTimer <= 0f)
                    {
                        playerStat.speed = PlayerSpeedBox;
                        FrozenTimer = MainFrozenTimer;
                    }                                       
                    break;
                   
            }
        }
        // Player 또는 Monster에 충돌했을 때
        else if (other.gameObject.CompareTag("Monster") || other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Ground"))
        {
                Debug.Log($"빗나감");           
                Destroy();
        }
    }

    private void Destroy()
    {
        // 1. 폭발 이펙트 생성
        Instantiate(HealthEffectPrefab, transform.position, Quaternion.identity);
        
        Destroy(this.gameObject);
    }
    private void HealthDestroy()
    {
        Instantiate(HealthEffectPrefab, transform.position, Quaternion.identity);
        HealthEffect.Play();
        Destroy(this.gameObject);
    }
    private void SmokeDestroy()
    {
        Instantiate(SmokeEffectPrefab, transform.position, Quaternion.identity);
        SmokeEffect.Play();
        Destroy(this.gameObject);
    }
    private void BoomDestroy()
    {
        Instantiate(BoomEffectPrefab, transform.position, Quaternion.identity);
        BoomEffect.Play();
        Destroy(this.gameObject);
    }
    private void FrozenDestroy()
    {
        Instantiate(FrozenEffectPrefab, transform.position, Quaternion.identity);
        FrozenEffect.Play();
        Destroy(this.gameObject);
    }
}


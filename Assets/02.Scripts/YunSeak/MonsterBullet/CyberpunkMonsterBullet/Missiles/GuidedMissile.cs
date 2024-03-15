using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuidedMissile : MonoBehaviour
{
    [Range(0, 1)]
    public int Health;
    public int MaxHealth = 1;

    [SerializeField] ParticleSystem BoomEffect = null;
    public float Search = 100f;
    public float damageToPlayer = 2f;

    private Transform player;
    private NavMeshAgent _navMeshAgent;
    public float AttackDistance;  // 공격 범위 
    public GameObject explosionPrefab;  // 폭발 이펙트 프리팹

    public float rotationSpeed = 500f;
    public float missilespeed = 5f;

    private Rigidbody _rigidbody;
    public float gravityTimer = 0.5f; // 그래비티를 활성화한 후 작동할 시간
    private float gravityTimerBox;
    // 2초동안 미사일 폭파 금지 코드 작성

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();

        // 시작시 그래비티 활성화
        _rigidbody.useGravity = true;

        gravityTimerBox = gravityTimer;
    }

    void Update()
    {
        Debug.Log(gravityTimer);
        gravityTimer -= Time.deltaTime;
        // 플레이어가 존재하면
        if (player != null)
        {
            // 몬스터가 플레이어를 향해 바라보도록 합니다.
            transform.LookAt(player.transform);

            transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime);

            if(gravityTimer <= 0)
            {    
                // missile의 Rotation 의 X값을 90도 바꾸는 코드 필요
                // 플레이어에게 다가간다.
                Vector3 direction = player.position - transform.position;
                direction.Normalize();
                transform.position += direction * missilespeed * Time.deltaTime;
            }

            // 내비게이션 설정
            _navMeshAgent.stoppingDistance = AttackDistance;
            _navMeshAgent.destination = player.position;
        }

        // 그래비티 타이머 업데이트
        if (gravityTimer <= 0f)
        {
            DisableGravity();
            gravityTimer = gravityTimerBox;
        }

        if (Health <= 0)
        {
            DestroyMonster();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("충돌");
            PlayerStat playerStat = other.GetComponent<PlayerStat>();
            // playerStat의 playerhealth 값을 가져와서 비교합니다.
            if (playerStat.playerhealth >= 0)
            {
                DestroyMonster();
                Debug.Log($"플레이어{damageToPlayer}피격");
                playerStat.playerhealth -= damageToPlayer;
            }
            else if (Health <= 0)
            {
                Debug.Log("Monster3_Fly 피격");
                DestroyMonster();
            }
        }
        else if (other.CompareTag("Wall") || other.CompareTag("Monster") || other.CompareTag("Grund") || other.CompareTag("MonsterBullet"))
        {
            DestroyMonster();
        }
        else if (other.CompareTag("Bullet"))
        {
            DestroyMonster();
        }
    }

    private void DestroyMonster()
    {
        if(gravityTimer <= 0f)
        {
            Debug.Log("미사일 폭발작동");
            // 1. 폭발 이펙트 생성
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            // 2. BoomEffect 재생
            BoomEffect.Play();
            // 3. Monster3_Fly 게임 오브젝트 파괴
            Destroy(gameObject);
        }
         
    }

    // 그래비티를 일정 시간동안 비활성화하는 함수
    private void DisableGravity()
    {
        Debug.Log("중력 x");
        _rigidbody.useGravity = false;
    }
}
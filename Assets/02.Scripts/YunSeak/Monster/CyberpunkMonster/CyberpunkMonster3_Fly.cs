using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CyberpunkMonster3_Fly : MonoBehaviour
{
    public float MaxHealth;
    public float Health = 1;
    public float rotationSpeed = 500f;
    public float speed = 5f;
    private Transform player;
    private NavMeshAgent _navMeshAgent;
    public float AttackDistance = 2f;  // 공격 범위 
    public GameObject explosionPrefab;  // 폭발 이펙트 프리팹
    public int damageToPlayer = 2;      // 플레이어에게 입힐 데미지
    [SerializeField] ParticleSystem BoomEffect = null;

    // 패트롤 기능 추가 작성 필요

    void Start()
    {        
        player = GameObject.FindGameObjectWithTag("Player").transform;
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime);

        // 플레이어에게 다가간다.
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        transform.position += direction * speed * Time.deltaTime;

        // 내비게이션 설정
        _navMeshAgent.stoppingDistance = AttackDistance;
        _navMeshAgent.destination = player.position;
    }

    // 충돌 처리
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
        else if (other.CompareTag("Wall") || other.CompareTag("Monster") || other.CompareTag("Grund"))
        {
            DestroyMonster();
        }
    }

    private void DestroyMonster()
    {
        // 1. 폭발 이펙트 생성
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        // 2. BoomEffect 재생
        BoomEffect.Play();
        // 3. Monster3_Fly 게임 오브젝트 파괴
        Destroy(gameObject);
    }
}
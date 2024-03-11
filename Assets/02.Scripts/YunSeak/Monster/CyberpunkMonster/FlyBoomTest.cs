using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyBoomTest : MonoBehaviour
{
    public float rotationSpeed = 500f;
    public float speed = 5f;
    private Transform _target;
    private Transform player;
    private NavMeshAgent _navMeshAgent;
    public float AttackDistance = 2f;  // 공격 범위 
    public GameObject explosionPrefab;  // 폭발 이펙트 프리팹
    public int damageToPlayer = 2;      // 플레이어에게 입힐 데미지
    private Collider playerCollider;     // 플레이어 콜라이더

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        playerCollider = player.GetComponent<Collider>();
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
        _navMeshAgent.destination = _target.position;
    }

    // 충돌 처리
    void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning(other);
        if (other == playerCollider)
        {
            // 플레이어와 충돌했을 때 처리

            // 1. 폭발 이펙트 생성
            //Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            //// 2. 플레이어의 체력 감소
            //PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            //if (playerHealth != null)
            //{
            //    playerHealth.TakeDamage(damageToPlayer);
            //}

            // 3. Boom 게임 오브젝트 파괴

            Destroy(gameObject);
        }
    }
}
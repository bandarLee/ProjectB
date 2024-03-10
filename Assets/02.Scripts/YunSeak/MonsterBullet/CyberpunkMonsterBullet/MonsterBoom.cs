using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MonsterBoom : MonoBehaviour
{
    // 자폭 몬스터 
    // 체력 2개 
    // 플레이어가 공격범위안에 있으면 추적하여 플레이어 콜라이더에 닿으면 1초 뒤 폭발
    // 플레이어가 자폭몬스터를 베면 한 번 넉백한 뒤 다시 추적, 다시 한번 공격당할 시 3초 뒤 폭발 
    // 폭발 범위에 있을 경우 데미지 발생

    [Range(0, 2)]
    public int Health;
    public int MaxHealth = 2;
    public Slider HealthSliderUI;

    private Transform _target;         // 플레이어
    public float FindDistance = 5f;  // 감지 거리
    public float AttackDistance = 2f;  // 공격 범위 
    public float MoveSpeed = 4f;  // 이동 상태
    public Vector3 StartPosition;     // 시작 위치
    public float MoveDistance = 40f; // 움직일 수 있는 거리

    private NavMeshAgent _navMeshAgent;
    private Rigidbody _rigidbody;
    public int Damage = 10; // 데미지
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

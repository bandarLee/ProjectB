using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public enum MonsterType // 몬스터의 종류
{
    Follow,
    Fire
}

public class MonsterFly : MonoBehaviour
{
    [Range(0, 100)]
    public int Health;
    public int MaxHealth = 100;
    public Slider HealthSliderUI;
    public MonsterType Type = MonsterType.Follow;
    /***********************************************************************/

    //private CharacterController _characterController;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    private Transform _target;         // 플레이어
    public float FindDistance = 5f;  // 감지 거리
    public float AttackDistance = 2f;  // 공격 범위 
    public float MoveSpeed = 4f;  // 이동 상태
    public Vector3 StartPosition;     // 시작 위치
    public float MoveDistance = 40f; // 움직일 수 있는 거리

    public const float TOLERANCE = 0.1f;

    public const float AttackDelay = 1f;


    public float KnockbackPower = 1.2f;

    private const float IDLE_DURATION = 3f;
    private float _idleTimer;
    public Transform PatrolTarget;


    private MonsterState _currentState = MonsterState.Idle;

    /******************************************************************************/

    // 목표: 폭발 범위 데미지 기능 구현
    // 필요 속성:
    // - 범위
    public float ExplosionRadius = 3;
    // 구현 순서:


    public int Damage = 60;

    public GameObject BombEffectPrefab;

    private Collider[] _colliders = new Collider[10];

    private void Start()
    {
        //_characterController = GetComponent<CharacterController>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = MoveSpeed;

        _animator = GetComponentInChildren<Animator>();


        _target = GameObject.FindGameObjectWithTag("Player").transform;

        StartPosition = transform.position;

        Init();
    }

    public void Init()
    {
        //_idleTimer = 0f;
        Health = MaxHealth;
    }
    void Update()
    {
        if (MonsterType.Follow == Type) 
        {
            switch (_currentState)
            {
                case MonsterState.Idle:
                    Idle();
                    break;

                case MonsterState.Patrol:
                    Patrol();
                    break;

                case MonsterState.Trace:
                    Trace();
                    break;

                case MonsterState.Comeback:
                    Comeback();
                    break;
            }
        }
        else if (MonsterType.Fire == Type) 
        {
            switch (_currentState)
            {
                case MonsterState.Idle:
                    Idle();
                    break;

                case MonsterState.Patrol:
                    Patrol();
                    break;

                case MonsterState.Trace:
                    GunFire();
                    break;

                case MonsterState.Comeback:
                    Comeback();
                    break;
            }
        }
    }

    private void Idle()
    {
        _idleTimer += Time.deltaTime;

        if (PatrolTarget != null && _idleTimer >= IDLE_DURATION)
        {
            _idleTimer = 0f;
            _animator.SetTrigger("IdleToPatrol");
            Debug.Log("상태 전환: Idle -> Patrol");
            _currentState = MonsterState.Patrol;
        }


        // todo: 몬스터의 Idle 애니메이션 재생
        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            Debug.Log("상태 전환: Idle -> Trace");
            _animator.SetTrigger("IdleToTrace");
            _currentState = MonsterState.Trace;
        }
    }
    private void Trace()
    {
        // Trace 상태일때의 행동 코드를 작성

        // 플레이어게 다가간다.
        // 1. 방향을 구한다. (target - me)
        Vector3 dir = _target.transform.position - this.transform.position;
        dir.y = 0;
        dir.Normalize();
        // 2. 이동한다.
        // _characterController.Move(dir * MoveSpeed * Time.deltaTime);

        // 내비게이션이 접근하는 최소 거리를 공격 가능 거리로 설정
        _navMeshAgent.stoppingDistance = AttackDistance;

        // 내비게이션의 목적지를 플레이어의 위치로 한다.
        _navMeshAgent.destination = _target.position;

        // 3. 쳐다본다.
        //transform.forward = dir; //(_target);

        if (Vector3.Distance(transform.position, StartPosition) >= MoveDistance)
        {
            Debug.Log("상태 전환: Trace -> Comeback");
            _animator.SetTrigger("TraceToComeback");
            _currentState = MonsterState.Comeback;
        }

        if (Vector3.Distance(_target.position, transform.position) <= AttackDistance)
        {
            Debug.Log("상태 전환: Trace -> Attack");
            _animator.SetTrigger("TraceToAttack");
            _currentState = MonsterState.Attack;
        }
    }

    private void Patrol()
    {
        
        {
            _navMeshAgent.stoppingDistance = 0f;
            _navMeshAgent.SetDestination(PatrolTarget.position);

            if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= TOLERANCE)
            {
                Debug.Log("상태 전환: Patrol -> Comeback");
                _animator.SetTrigger("PatrolToComeback");
                _currentState = MonsterState.Comeback;
            }

            if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
            {
                Debug.Log("상태 전환: Patrol -> Trace");
                _animator.SetTrigger("PatrolToTrace");
                _currentState = MonsterState.Trace;
            }
        }
    }


    private void Comeback()
    {
        // 실습 과제 34. 복귀 상태의 행동 구현하기:
        // 시작 지점 쳐다보면서 시작지점으로 이동하기 (이동 완료하면 다시 Idle 상태로 전환)
        // 1. 방향을 구한다. (target - me)
        Vector3 dir = StartPosition - this.transform.position;
        dir.y = 0;
        dir.Normalize();
        // 2. 이동한다.
        //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
        // 3. 쳐다본다.
        //transform.forward = dir; //(_target);

        // 내비게이션이 접근하는 최소 거리를 오차 범위
        _navMeshAgent.stoppingDistance = TOLERANCE;

        // 내비게이션의 목적지를 플레이어의 위치로 한다.
        _navMeshAgent.destination = StartPosition;

        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= TOLERANCE)
        {
            Debug.Log("상태 전환: Comeback -> idle");
            _animator.SetTrigger("ComebackToIdle");
            _currentState = MonsterState.Idle;
        }

        if (Vector3.Distance(StartPosition, transform.position) <= TOLERANCE)
        {
            Debug.Log("상태 전환: Comeback -> idle");
            _animator.SetTrigger("ComebackToIdle");
            _currentState = MonsterState.Idle;
        }
    }
    private void GunFire()
    {



    }
    // 1. 터질 때
    private void OnColliderEnter(Collider other)
    {
        
        gameObject.SetActive(false); // 창고에 넣는다.

        GameObject effect = Instantiate(BombEffectPrefab);
        effect.transform.position = this.gameObject.transform.position;

        // 2. 범위안에 있는 모든 콜라이더를 찾는다.
        // -> 피직스.오버랩 함수는 특정 영역(radius) 안에 있는 특정 레이어들의 게임 오브젝트의
        //    콜라이더 컴포넌트들을 모두 찾아 배열로 반환하는 함수
        // 영역의 형태: 스피어, 큐브, 캡슐
        int layer =/* LayerMask.GetMask("Player") |*/ LayerMask.GetMask("Plaer");
        int count = Physics.OverlapSphereNonAlloc(transform.position, ExplosionRadius, _colliders, layer);
        // 3. 찾은 콜라이더 중에서 타격 가능한(IHitable) 오브젝트를 찾아서 Hit()한다.
    }
}
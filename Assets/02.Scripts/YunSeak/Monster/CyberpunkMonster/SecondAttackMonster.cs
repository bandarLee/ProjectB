using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

// Monster1 / 선공/후공
// 정해진 장소로 순찰을 돌며 플레이어 탐색
// 플레이어가 추적범위내에 들어왔으면 추적
// 플레이어와 콜라이더가 충돌하면 격투
// 플레이어에게 데미지를 입으면 넉백 + 체력감소
// 플레이어가 공중으로 날으면 가장 레이 캐스트로 감지한 가장 가까운 벽을 타고 오른다
// 플레이어 방향으로 날아가며 공격
// 체력이 없을 경우 3초간 정지 후 폭발
// 폭발 범위에 있을 경우 데미지 발생

// Monster4 / 선공몹
// 정해진 장소로 순찰을 돌며 플레이어 탐색
// 플레이어가 추적범위내에 들어왔으면 플레이어를 향해 사격
// 사격은 2초에 1발씩 발사
// 플레이어와 콜라이더가 충돌하면 격투
// 플레이어에게 데미지를 입으면 넉백 + 체력감소
// 체력이 없을 경우 3초간 정지 후 폭발
// 폭발 범위에 있을 경우 데미지 발생

// Monster5 / 후공몹
// 정해진 장소로 순찰을 돌며 플레이어 탐색
// 플레이어가 추적범위내에 들어왔으면 추적 + 사격
// 사격은 2초에 1발씩 발사
// 플레이어와 콜라이더가 충돌하면 격투
// 플레이어에게 데미지를 입으면 넉백 + 체력감소
// 플레이어가 공중으로 날으면 가장 레이 캐스트로 감지한 가장 가까운 벽을 타고 오른다
// 플레이어 방향으로 날아가며 공격
// 체력이 없을 경우 3초간 정지 후 폭발
// 폭발 범위에 있을 경우 데미지 발생

public enum MonsterState // 몬스터의 상태
{
    Idle,           // 대기
    Patrol,         // 순찰
    Trace,          // 추적 / 원격사격
    Attack,         // 공격 / 근접
    FireAttack,     // 총격
    Comeback,       // 복귀
    Damaged,        // 피격
    Die             // 사망
}

public class SecondAttackMonster : MonoBehaviour
{

    [Range(0, 100)]
    public int Health;
    public int MaxHealth = 100;
    public Slider HealthSliderUI;
    /***********************************************************************/

    //private CharacterController _characterController;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private Rigidbody _rigidbody;

    private Transform _target;         // 플레이어
    public float FindDistance = 5f;  // 감지 거리
    public float AttackDistance = 2f;  // 공격 범위 
    public float MoveSpeed = 4f;  // 이동 상태
    public Vector3 StartPosition;     // 시작 위치
    public float MoveDistance = 40f; // 움직일 수 있는 거리
    //public float Fire =
    private float lastAttackTime = 0f;
    public float attackDelay = 1.15f;

    public const float TOLERANCE = 0.1f;
    public int Damage = 10;
    public const float AttackDelay = 1f;
    private float _attackTimer = 0f;

    private Vector3 _knockbackStartPosition;
    private Vector3 _knockbackEndPosition;
    private const float KNOCKBACK_DURATION = 0.1f;
    private float _knockbackProgress = 0f;
    public float KnockbackPower = 1.2f;

    private const float IDLE_DURATION = 3f;
    private float _idleTimer;
    public Transform PatrolTarget;


    private MonsterState _currentState = MonsterState.Idle;



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
        _idleTimer = 0f;
        Health = MaxHealth;
    }

    private void Update()
    {
        //HealthSliderUI.value = (float)Health / (float)MaxHealth;  // 0 ~ 1
        //Debug.Log(Health);
        if (Health <= 0)
        {
            Destroy(this.gameObject);
        }

        // 1. 만약 벽에 닿아 있는데 && 스태미너가 > 0
        //if (Stamina > 0 && _characterController.collisionFlags == CollisionFlags.Sides)
        //{
        //    // 2. [Spacebar] 버튼을 누르고 있으면
        //    if (Input.GetKey(KeyCode.Space))
        //    {
        //        // 3. 벽을 타겠다.
        //        _isClimbing = true;
        //        _yVelocity = ClimbingPower;

        //    }
        //}
        // 상태 패턴: 상태에 따라 행동을 다르게 하는 패턴 
        // 1. 몬스터가 가질 수 있는 행동에 따라 상태를 나눈다.
        // 2. 상태들이 조건에 따라 자연스럽게 전환(Transition)되게 설계한다.

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

            case MonsterState.Attack:
                Attack(); 
                break;

            case MonsterState.FireAttack:
                FireAttack();
                break;

            case MonsterState.Comeback:
                Comeback();
                break;
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
    private void Attack()
    {
        // 전이 사건: 플레이어와 거리가 공격 범위보다 멀어지면 다시 Trace
        if (Vector3.Distance(_target.position, transform.position) > AttackDistance)
        {
            _attackTimer = 0f;
            Debug.Log("상태 전환: Attack -> Trace");
            _animator.SetTrigger("AttackToTrace");
            _currentState = MonsterState.Trace;
            return;
        }

        // 실습 과제 35. Attack 상태일 때 N초에 한 번 때리게 딜레이 주기
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= AttackDelay)
        {
            _animator.SetTrigger("Attack");
            _animator.Play("Attack");
        }
    }
    private void FireAttack()
    {

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

    private void Damaged()
    {
        // 1. Damage 애니메이션 실행(0.5초)
        // todo: 애니메이션 실행
        
        // 2. 넉백 구현
        // 2-1. 넉백 시작/최종 위치를 구한다.
        if (_knockbackProgress == 0)
        {
            _knockbackStartPosition = transform.position;

            Vector3 dir = transform.position - _target.position;
            dir.y = 0;
            dir.Normalize();

            _knockbackEndPosition = transform.position + dir * KnockbackPower;
        }

        _knockbackProgress += Time.deltaTime / KNOCKBACK_DURATION;

        // 2-2. Lerp를 이용해 넉백하기
        transform.position = Vector3.Lerp(_knockbackStartPosition, _knockbackEndPosition, _knockbackProgress);

        if (_knockbackProgress > 1)
        {
            _knockbackProgress = 0f;

            Debug.Log("상태 전환: Damaged -> Trace");
            _animator.SetTrigger("DamagedToTrace");
            _currentState = MonsterState.Trace;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        {
            if (other.gameObject.tag == "basicweapon" && Time.time >= lastAttackTime + attackDelay)
            {
                Health--;
                lastAttackTime = Time.time;

            }
        }
    }

}
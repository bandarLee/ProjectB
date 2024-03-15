using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public enum CyberpunkMonster1_TestState // 몬스터의 상태
{
    Idle,           // 대기
    Patrol,         // 순찰
    Trace,          // 추적
    Attack,         // 공격
    Comeback,       // 복귀
//    JumpAttack,     // 점프 공격
//    Landing,        // 착지
    Damaged,        // 공격 당함
    Die             // 사망
}

public class CyberpunkMonster1_Test : MonoBehaviour, IHitable
{
    [Range(0, 3)]
    public int Health;
    public int MaxHealth = 3;
    public Slider HealthSliderUI;
    /***********************************************************************/

    private CharacterController _characterController;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;


    private Transform _target;         // 플레이어
    public float FindDistance = 5f;  // 감지 거리
    public float AttackDistance = 2f;  // 공격 범위 
    public float MoveSpeed = 10f;  // 이동 상태
    private float MoveBox;
    public Vector3 StartPosition;     // 시작 위치
    public float MoveDistance = 40f; // 움직일 수 있는 거리
    public const float TOLERANCE = 0.1f;
    public int Damage = 10;
    public const float attackDelay = 1.15f;
    private float _attackTimer = 0f;

    private Vector3 _knockbackStartPosition;
    private Vector3 _knockbackEndPosition;
    private const float KNOCKBACK_DURATION = 0.1f;
    private float _knockbackProgress = 0f;
    public float KnockbackPower = 1.2f;

    private const float IDLE_DURATION = 3f;
    private float _idleTimer;
    public Transform[] PatrolTarget;  // 몬스터가 순찰하는 지점들의 배열
    private int currentPatrolIndex = 0; // 현재 순찰 중인 인덱스를 저장하는 변수

    private float lastAttackTime = 0f;
    public float PlayerFlyAtteckLimitSensor = 3f;// 플레이어 플라이 혹은 점프시 공격

    private CyberpunkMonster1_TestState _cyberpunkMonster1_TestState = CyberpunkMonster1_TestState.Idle;



    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = MoveSpeed;

        _animator = GetComponentInChildren<Animator>();


        _target = GameObject.FindGameObjectWithTag("Player").transform;

        StartPosition = transform.position;
        MoveBox = MoveSpeed;

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

        switch (_cyberpunkMonster1_TestState)
        {
            case CyberpunkMonster1_TestState.Idle:
                Idle();
                break;

            case CyberpunkMonster1_TestState.Patrol:
                Patrol();
                break;

            case CyberpunkMonster1_TestState.Trace:
                Trace();
                break;

            case CyberpunkMonster1_TestState.Attack:
                Attack();
                break;

            //case CyberpunkMonster1_TestState.JumpAttack:
            //    JumpAttack();
            //    break;

            //case CyberpunkMonster1_TestState.Landing:
            //    Landing();
            //    break;
            case CyberpunkMonster1_TestState.Comeback:
                Comeback();
                break;

            case CyberpunkMonster1_TestState.Damaged:
                Damaged();
                break;

            case CyberpunkMonster1_TestState.Die:
                Die();
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
            _cyberpunkMonster1_TestState = CyberpunkMonster1_TestState.Patrol;
        }


        // todo: 몬스터의 Idle 애니메이션 재생
        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            Debug.Log("상태 전환: Idle -> Trace");
            _animator.SetTrigger("IdleToTrace");
            _cyberpunkMonster1_TestState = CyberpunkMonster1_TestState.Trace;
        }
    }

    private void Patrol()
    {
        MoveSpeed = MoveBox;
        _navMeshAgent.stoppingDistance = 0f;

        // PatrolTarget 배열이 유효하고 최소한 하나의 위치가 있을 때
        if (PatrolTarget != null && PatrolTarget.Length > 0)
        {
            // 현재 순찰 중인 인덱스에 해당하는 PatrolTarget의 위치로 몬스터를 이동
            _navMeshAgent.SetDestination(PatrolTarget[currentPatrolIndex].position);
            // 다음 순찰 인덱스로 이동
            currentPatrolIndex = (currentPatrolIndex + 1) % PatrolTarget.Length;
            Debug.Log("상태 전환: Patrol1 -> Patrol2");
            _animator.SetTrigger("Patrol");
            _cyberpunkMonster1_TestState = CyberpunkMonster1_TestState.Patrol;
        }
        else
        {
            Debug.LogWarning("PatrolTarget이 설정되지 않았거나 PatrolTarget이 비어 있습니다.");
        }

        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= TOLERANCE)
        {
            Debug.Log("상태 전환: Patrol -> Comeback");
            _animator.SetTrigger("PatrolToComeback");
            _cyberpunkMonster1_TestState = CyberpunkMonster1_TestState.Comeback;
        }

        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            Debug.Log("상태 전환: Patrol -> Trace");
            _animator.SetTrigger("PatrolToTrace");
            _cyberpunkMonster1_TestState = CyberpunkMonster1_TestState.Trace;
        }
    }
    private void TurnPatrol()
    {
        _animator.SetTrigger("PatrolToTrace");
    }
    private void Trace()
    {
        MoveSpeed = 5f;
        // 플레이어게 다가간다.
        // 1. 방향을 구한다. (target - me)
        Vector3 dir = _target.transform.position - this.transform.position;
        dir.y = 0;
        dir.Normalize();
        // 2. 이동한다.
        _navMeshAgent.stoppingDistance = AttackDistance;

        // 내비게이션의 목적지를 플레이어의 위치로 한다.
        _navMeshAgent.destination = _target.position;

        if (Vector3.Distance(transform.position, StartPosition) >= MoveDistance)
        {
            Debug.Log("상태 전환: Trace -> Comeback");
            _animator.SetTrigger("TraceToComeback");
            _cyberpunkMonster1_TestState = CyberpunkMonster1_TestState.Comeback;
        }

        if (Vector3.Distance(_target.position, transform.position) <= AttackDistance)
        {
            Debug.Log("상태 전환: Trace -> Attack");
            _animator.SetTrigger("TraceToAttack");
            RrahTrace();
            _cyberpunkMonster1_TestState = CyberpunkMonster1_TestState.Attack;
        }
    }
    private void RrahTrace()
    {
        _animator.SetTrigger("RrahTrace");
    }

    private void Attack()
    {
        // 전이 사건: 플레이어와 거리가 공격 범위보다 멀어지면 다시 Trace
        if (Vector3.Distance(_target.position, transform.position) > AttackDistance)
        {
            _attackTimer = 0f;
            Debug.Log("상태 전환: Attack -> Trace");
            _animator.SetTrigger("AttackToTrace");
            _cyberpunkMonster1_TestState = CyberpunkMonster1_TestState.Trace;
            return;
        }

        // Attack 상태일 때 N초에 한 번 때리게 딜레이 주기
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= attackDelay)
        {
            _animator.SetTrigger("Attack");
            _animator.Play("Attack");
        }
    }
    
    //private void JumpAttack()
    //{
    //    // 플레이어의 위치를 가져옴
    //    Vector3 playerPosition = _target.position;

    //    // 플레이어의 Y축 값이 3보다 높은지 확인
    //    if (playerPosition.y > PlayerFlyAtteckLimitSensor)
    //    {
    //        _navMeshAgent.destination = playerPosition;
    //        Debug.Log("상태 전환: JumpAttack -> Landing");
    //        _animator.SetTrigger("JumpAttackToLanding");
    //        _cyberpunkMonster1_TestState = CyberpunkMonster1_TestState.Landing;
    //        Attack();
    //    }
    //}

    //private void Landing()
    //{
    //    // 게임오브젝트가 그라운드에 착지하면
    //    if (_characterController.isGrounded)
    //    {
    //        // 0.3초 동안 Landing 애니메이션 출력
    //        Debug.Log("상태 전환: Landing -> Trace");
    //        _animator.SetTrigger("LandingToTrace");
    //        _cyberpunkMonster1_TestState = CyberpunkMonster1_TestState.Trace;
    //        StartCoroutine(WaitForLandingAnimation());
    //    }
    //}

    //private IEnumerator WaitForLandingAnimation()
    //{
    //    // 0.3초 대기
    //    yield return new WaitForSeconds(0.3f);

    //    // Landing 애니메이션 종료 후 Trace 상태로 전환하여 공격 시작
    //    Debug.Log("상태 전환: JumpAttack -> Trace");
    //    _animator.SetTrigger("JumpAttackToTrace");
    //    _cyberpunkMonster1_TestState = CyberpunkMonster1_TestState.Trace;
    //    Attack();
    //}

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
            _cyberpunkMonster1_TestState = CyberpunkMonster1_TestState.Idle;
        }

        if (Vector3.Distance(StartPosition, transform.position) <= TOLERANCE)
        {
            Debug.Log("상태 전환: Comeback -> idle");
            _animator.SetTrigger("ComebackToIdle");
            _cyberpunkMonster1_TestState = CyberpunkMonster1_TestState.Idle;
        }
    }

    private void Damaged()
    {
        // 데미지 애니메이션 실행과 넉백 구현
        if (_knockbackProgress == 0)
        {
            // 넉백 시작 위치를 현재 위치로 설정합니다.
            _knockbackStartPosition = transform.position;

            // 데미지를 주는 대상에서 현재 위치를 빼 방향을 구합니다.
            Vector3 dir = transform.position - _target.position;
            dir.y = 0;
            dir.Normalize();

            // 넉백의 최종 위치를 계산합니다.
            _knockbackEndPosition = transform.position + dir * KnockbackPower;
        }

        // 넉백 진행도를 업데이트합니다.
        _knockbackProgress += Time.deltaTime / KNOCKBACK_DURATION;

        // 넉백을 진행합니다.
        transform.position = Vector3.Lerp(_knockbackStartPosition, _knockbackEndPosition, _knockbackProgress);

        // 넉백이 끝나면 Trace 상태로 전환합니다.
        if (_knockbackProgress > 1)
        {
            _knockbackProgress = 0f;

            Debug.Log("상태 전환: Damaged -> Trace");
            _animator.SetTrigger("DamagedToTrace");
            _cyberpunkMonster1_TestState = CyberpunkMonster1_TestState.Trace;
        }
    }
    public void Hit(DamageInfo damage)
    {
        if (_cyberpunkMonster1_TestState == CyberpunkMonster1_TestState.Die)
        {
            return;
        }

        Health -= damage.Amount;
        if (Health <= 0)
        {
            Debug.Log("상태 전환: Any -> Die");

            _animator.SetTrigger($"Die{Random.Range(0, 2)}");
            _cyberpunkMonster1_TestState = CyberpunkMonster1_TestState.Die;
        }
        else
        {
            Debug.Log("상태 전환: Any -> Damaged");

            _animator.SetTrigger("Damaged");
            _cyberpunkMonster1_TestState = CyberpunkMonster1_TestState.Damaged;
        }
    }
    private Coroutine _dieCoroutine;
    private void Die()
    {
        if (_dieCoroutine == null)
        {
            _dieCoroutine = StartCoroutine(Die_Coroutine());
        }
    }
    public void PlayerAttack()
    {
        IHitable playerHitable = _target.GetComponent<IHitable>();
        if (playerHitable != null)
        {
            Debug.Log("때렸다!");

            DamageInfo damageInfo = new DamageInfo(DamageType.Normal, Damage);
            playerHitable.Hit(damageInfo);
            _attackTimer = 0f;
        }
    }
    private IEnumerator Die_Coroutine()
    {
        _navMeshAgent.isStopped = true;
        _navMeshAgent.ResetPath();

        HealthSliderUI.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        Destroy(gameObject);

        // 죽을때 아이템 생성
    }
}

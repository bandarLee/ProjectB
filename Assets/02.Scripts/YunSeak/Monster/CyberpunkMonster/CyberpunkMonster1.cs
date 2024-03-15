using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum CyberpunkMonster1State // 몬스터의 상태
{
    Idle,           // 대기
    Patrol,         // 순찰
    Trace,          // 추적
    Attack,         // 공격
    Comeback,       // 복귀
    JumpAttack,     // 점프 공격
    Landing,        // 착지
    Damaged,        // 공격 당함
    Die             // 사망
}

public class CyberpunkMonster1 : MonoBehaviour
{
    [Range(0, 10)]
    public int Health;  // 몬스터의 현재 체력
    public int MaxHealth = 10;  // 몬스터의 최대 체력
    public Slider HealthSliderUI;  // UI 슬라이더를 통해 표시되는 체력 바

    /***********************************************************************/

    //private CharacterController _characterController;
    private NavMeshAgent _navMeshAgent;  // NavMesh 상에서 몬스터의 이동을 관리하는 NavMeshAgent
    private Animator _animator;  // 몬스터의 애니메이션을 제어하는 Animator

    private Transform _target;  // 몬스터가 추적하는 대상인 플레이어의 위치
    public float FindDistance;  // 플레이어를 감지할 수 있는 최대 거리
    public float AttackDistance;  // 플레이어를 공격할 수 있는 최대 거리
    public float MoveSpeed;  // 몬스터의 이동 속도
    public Vector3 StartPosition;  // 몬스터의 시작 위치
    public float MoveDistance = 40f;  // 몬스터가 이동할 수 있는 최대 거리
    public const float TOLERANCE = 0.1f;  // 거리 비교 시 사용되는 허용 오차
    public int MonsterAtteckDamage = 1;  // 몬스터의 공격력
    public const float AttackDelay = 1f;  // 몬스터의 공격 딜레이
    private float _attackTimer = 0f;  // 공격 딜레이를 계산하는 타이머
    public string playerTag = "Player"; // 플레이어 태그
    private GameObject player; // 플레이어 오브젝트
    public float PlayerFlyAtteckLimitSensor = 3f;// 플레이어 플라이 혹은 점프시 공격

    public int JumpAttackTimer;// 점프시간차후 애니메이션이 자연스러워지면 프라이빗으로 변경
    private CharacterController _characterController;
    private Vector3 _moveDirection = Vector3.zero;
    public float Gravity = 20.0f;// 중력

    private Vector3 _knockbackStartPosition;  // 넉백 효과의 시작 위치
    private Vector3 _knockbackEndPosition;  // 넉백 효과의 끝 위치
    private const float KNOCKBACK_DURATION = 0.1f;  // 넉백 지속 시간
    private float _knockbackProgress = 0f;  // 넉백 진행 상태
    public float KnockbackPower = 1.2f;  // 넉백의 세기

    private const float IDLE_DURATION = 3f;  // 대기 시간
    private float _idleTimer;  // 대기 타이머
    public Transform[] PatrolTarget;  // 몬스터가 순찰하는 지점들의 배열
    private int currentPatrolIndex = 0; // 현재 순찰 중인 PatrolTarget 배열의 인덱스

    private CyberpunkMonster1State _cyberpunkMonster1State = CyberpunkMonster1State.Idle;  // 몬스터의 현재 상태

    private void Start()
    {
        //_characterController = GetComponent<CharacterController>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = MoveSpeed;

        _animator = GetComponentInChildren<Animator>();

        _characterController = GetComponent<CharacterController>();

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
        HealthSliderUI.value = (float)Health / (float)MaxHealth;  // 0 ~ 1
                                                                  // 중력 적용
        _moveDirection.y -= Gravity * Time.deltaTime;

        // CharacterController를 사용하여 이동
        _characterController.Move(_moveDirection * Time.deltaTime);

        // 상태 패턴: 상태에 따라 행동을 다르게 하는 패턴 
        // 1. 몬스터가 가질 수 있는 행동에 따라 상태를 나눈다.
        // 2. 상태들이 조건에 따라 자연스럽게 전환(Transition)되게 설계한다.

        switch (_cyberpunkMonster1State)
        {
            case CyberpunkMonster1State.Idle:
                Idle();
                break;

            case CyberpunkMonster1State.Patrol:
                Patrol();
                break;

            case CyberpunkMonster1State.Trace:
                Trace();
                break;

            case CyberpunkMonster1State.Comeback:
                Comeback();
                break;

            case CyberpunkMonster1State.Attack:
                Attack();
                break;

            case CyberpunkMonster1State.JumpAttack:
                JumpAttack();
                break;

            case CyberpunkMonster1State.Landing:
                Landing();
                break;

            case CyberpunkMonster1State.Damaged:
                Damaged();
                break;

            case CyberpunkMonster1State.Die:
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
            _animator.SetTrigger($"Die{Random.Range(1, 5)}");
            _animator.SetTrigger("IdleToPatrol");
            Debug.Log("상태 전환: Idle -> Patrol");
            _cyberpunkMonster1State = CyberpunkMonster1State.Patrol;
        }

        // todo: 몬스터의 Idle 애니메이션 재생
        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            _animator.SetTrigger($"Die{Random.Range(1, 5)}");
            Debug.Log("상태 전환: Idle -> Trace");
            _animator.SetTrigger("IdleToTrace");
            _cyberpunkMonster1State = CyberpunkMonster1State.Trace;
        }
    }

    private void Trace()
    {
        // Trace 상태일때의 행동 코드를 작성

        // 플레이어게 다가간다.
        Vector3 dir = _target.transform.position - this.transform.position;
        dir.y = 0;
        dir.Normalize();

        _navMeshAgent.stoppingDistance = AttackDistance;

        // 내비게이션의 목적지를 플레이어의 위치로 한다.
        _navMeshAgent.destination = _target.position;

        if (Vector3.Distance(transform.position, StartPosition) >= MoveDistance)
        {
            Debug.Log("상태 전환: Trace -> Comeback");
            _animator.SetTrigger("TraceToComeback");
            _cyberpunkMonster1State = CyberpunkMonster1State.Comeback;
        }

        if (Vector3.Distance(_target.position, transform.position) <= AttackDistance)
        {
            _animator.SetTrigger($"Die{Random.Range(1, 6)}");
            Debug.Log("상태 전환: Trace -> Attack");
            _animator.SetTrigger("TraceToAttack");
            _cyberpunkMonster1State = CyberpunkMonster1State.Attack;
        }
    }

    private void Patrol()
    {
        _navMeshAgent.stoppingDistance = 0f;

        // PatrolTarget 배열이 유효하고 최소한 하나의 위치가 있을 때
        if (PatrolTarget != null && PatrolTarget.Length > 0)
        {
            // 현재 목표 위치에 도착했을 때
            if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= TOLERANCE)
            {
                // 다음 순찰 위치로 이동
                currentPatrolIndex = (currentPatrolIndex + 1) % PatrolTarget.Length;
                _navMeshAgent.destination = PatrolTarget[currentPatrolIndex].position;
            }

            // 플레이어가 감지되면 Trace 상태로 전환
            if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
            {
                Debug.Log("상태 전환: Patrol -> Trace");
                _animator.SetTrigger("PatrolToTrace");
                _cyberpunkMonster1State = CyberpunkMonster1State.Trace;
            }

            // 몬스터의 상태를 Patrol로 설정
            Debug.Log("상태 전환: Patrol -> Patrol");
            _animator.SetTrigger("Patrol");
            _cyberpunkMonster1State = CyberpunkMonster1State.Patrol;
        }
        else
        {
            Debug.LogWarning("PatrolTarget이 설정되지 않았거나 PatrolTarget이 비어 있습니다.");
        }
    }
    private void JumpAttack()
    {
        // 플레이어의 위치를 가져옴
        Vector3 playerPosition = _target.position;

        // 플레이어의 Y축 값이 3보다 높은지 확인
        if(playerPosition.y > PlayerFlyAtteckLimitSensor)
        {
            _navMeshAgent.destination = playerPosition;
            Debug.Log("상태 전환: JumpAttack -> Landing");
            _animator.SetTrigger("JumpAttackToLanding");
            _cyberpunkMonster1State = CyberpunkMonster1State.Landing;
            Attack();
        }        
    }

    private void Landing()
    {
        // 게임오브젝트가 그라운드에 착지하면
        if (_characterController.isGrounded)
        {
            // 0.3초 동안 Landing 애니메이션 출력
            Debug.Log("상태 전환: Landing -> Trace");
            _animator.SetTrigger("LandingToTrace");
            _cyberpunkMonster1State = CyberpunkMonster1State.Trace;
            StartCoroutine(WaitForLandingAnimation());
        }
    }

    private IEnumerator WaitForLandingAnimation()
    {
        // 0.3초 대기
        yield return new WaitForSeconds(0.3f);

        // Landing 애니메이션 종료 후 Trace 상태로 전환하여 공격 시작
        Debug.Log("상태 전환: JumpAttack -> Trace");
        _animator.SetTrigger("JumpAttackToTrace");
        _cyberpunkMonster1State = CyberpunkMonster1State.Trace;
        Attack();
    }

    private void Comeback()
    {
        // 실습 과제 34. 복귀 상태의 행동 구현하기:
        // 시작 지점 쳐다보면서 시작지점으로 이동하기 (이동 완료하면 다시 Idle 상태로 전환)
        // 1. 방향을 구한다. (target - me)
        Vector3 dir = StartPosition - this.transform.position;
        dir.y = 0;
        dir.Normalize();

        // 내비게이션이 접근하는 최소 거리를 오차 범위
        _navMeshAgent.stoppingDistance = TOLERANCE;

        // 내비게이션의 목적지를 플레이어의 위치로 한다.
        _navMeshAgent.destination = StartPosition;

        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= TOLERANCE)
        {
            Debug.Log("상태 전환: Comeback -> Patrol");
            _animator.SetTrigger("ComebackToPatrol");
            _cyberpunkMonster1State = CyberpunkMonster1State.Patrol;
        }

        if (Vector3.Distance(StartPosition, transform.position) <= TOLERANCE)
        {
            Debug.Log("상태 전환: Comeback -> idle");
            _animator.SetTrigger("ComebackToIdle");
            _cyberpunkMonster1State = CyberpunkMonster1State.Idle;
        }
    }

    private void Attack()
    {
        // 공격 범위를 벗어나면 Trace 상태로 전환
        if (Vector3.Distance(_target.position, transform.position) > AttackDistance)
        {
            _attackTimer = 0f;
            _animator.SetTrigger($"Die{Random.Range(1, 6)}");
            Debug.Log("상태 전환: Attack -> Trace");
            _animator.SetTrigger("AttackToTrace");
            _cyberpunkMonster1State = CyberpunkMonster1State.Trace;
            return;
        }

        // 일정 시간마다 공격 애니메이션 재생
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= AttackDelay)
        {
            _animator.SetTrigger("Attack");
            _animator.Play("Attack");
        }
    }

    // 플레이어가 몬스터의 공격 범위에 진입했을 때 호출되는 이벤트 처리
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStat playerStat = other.GetComponent<PlayerStat>();
            if (playerStat != null)
            {
                // 플레이어에게 공격
                playerStat.playerhealth -= MonsterAtteckDamage;
            }
        }
        // 플레이어의 위치를 가져옴
        Vector3 playerPosition = _target.position;

        // 플레이어의 Y축 값이 PlayerFlyAtteckLimitSensor보다 높은지 확인
        if (playerPosition.y > PlayerFlyAtteckLimitSensor)
        {
            // JumpAttack 애니메이션 출력
            _animator.SetTrigger("AttackToJumpAttack");
        }
    }

    public void PlayerAttack()
    {
        IHitable playerHitable = _target.GetComponent<IHitable>();
        if (playerHitable != null)
        {
            Debug.Log("때렸다!");

            DamageInfo damageInfo = new DamageInfo(DamageType.Normal, MonsterAtteckDamage);
            playerHitable.Hit(damageInfo);
            _attackTimer = 0f;
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
            _animator.SetTrigger($"Die{Random.Range(1, 3)}");
            Debug.Log("상태 전환: Damaged -> Trace");
            _animator.SetTrigger("DamagedToTrace");
            _cyberpunkMonster1State = CyberpunkMonster1State.Trace;
        }
    }

    public void Hit(DamageInfo damage)
    {
        if (_cyberpunkMonster1State == CyberpunkMonster1State.Die)
        {
            return;
        }

        Health -= damage.Amount;
        if (Health <= 0)
        {
            Debug.Log("상태 전환: Any -> Die");

            _animator.SetTrigger($"Die{Random.Range(1, 3)}");
            _cyberpunkMonster1State = CyberpunkMonster1State.Die;
        }
        else
        {
            Debug.Log("상태 전환: Any -> Damaged");

            _animator.SetTrigger("Damaged");
            _cyberpunkMonster1State = CyberpunkMonster1State.Damaged;
        }
    }

    private Coroutine _dieCoroutine;
    private void Die()
    {
        // 매 프레임마다 해야 할 행동을 추가

        if (_dieCoroutine == null)
        {
            _dieCoroutine = StartCoroutine(Die_Coroutine());
        }
    }

    private IEnumerator Die_Coroutine()
    {
        _navMeshAgent.isStopped = true;
        _navMeshAgent.ResetPath();

        HealthSliderUI.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        Destroy(gameObject);

        // 죽을때 랜덤 아이템 생성
    }
}

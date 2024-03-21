using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum TeampleMonsterState // 몬스터의 상태
{
    Idle,           // 대기
    Patrol,         // 순찰
    Attack,         // 공격
    Comeback,       // 복귀
    Die             // 사망
}

public class TeampleMonster : MonoBehaviour
{
    public float health = 2;
    public float attackDelay = 0.6f;
    private float lastAttackTime = 0f;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    private float attackRange = 20f;
    private bool isShooting = true;
    private float attackGracePeriod = 2f;
    private float lastTimePlayerInRange;
    GameObject player;
    public Slider healthBarSlider;
    public GameObject healthBarUI;
    public TextMeshProUGUI damage;

    public Transform patrolTarget1;  // 몬스터가 순찰하는 지점1
    public Transform patrolTarget2;  // 몬스터가 순찰하는 지점2
    private Animator _animator;
    private TeampleMonsterState _teampleMonsterState = TeampleMonsterState.Idle;
    public float moveSpeed;
    private float moveBox;

    private const float IDLE_DURATION = 3f;// 대기 상태에서의 최대 대기 시간
    private float _idleTimer;// 대기 상태에서 경과한 시간을 추적하는 타이머
    public float DieTimer;// 사망 타이머
    public GameObject swordauraprefab;
    public Transform strongAttackpoint;
    private void Start()
    {

        player = GameObject.FindGameObjectWithTag("PlayerHead");

        healthBarUI.SetActive(false);
        InitializeHealthBar();
        StartCoroutine(ShootAtPlayerAfterDelay(2));
        moveBox = moveSpeed;

        // _animator 초기화
        _animator = GetComponentInChildren<Animator>();
    }

    void UpdateIdleTimer()
    {
        if (_teampleMonsterState == TeampleMonsterState.Idle)
        {
            _idleTimer += Time.deltaTime;
        }
    }
    void Update()
    {
        switch (_teampleMonsterState)
        {
            case TeampleMonsterState.Idle:
                Idle();
                break;

            case TeampleMonsterState.Patrol:
                Patrol();
                break;

            case TeampleMonsterState.Attack:
                Attack();
                break;

            case TeampleMonsterState.Comeback:
                Comeback();
                break;

            case TeampleMonsterState.Die:
                Die();
                break;
        }
        UpdateHealthBar();
        UpdateIdleTimer(); // _idleTimer를 업데이트합니다.
    }

    private void Idle()
    {
        // _animator가 초기화되지 않았는지 확인
        if (_animator == null)
        {
            Debug.LogError("에러다");
            return;
        }

        _idleTimer += Time.deltaTime;
        if (patrolTarget1 != null && _idleTimer >= IDLE_DURATION)
        {
            Debug.Log("상태 전환: Idle-> Patrol");
            _animator.SetTrigger("IdleToPatrol");
            _teampleMonsterState = TeampleMonsterState.Patrol;
        }
    }
    private void Patrol()
    {
        // 몬스터의 이동 속도를 원래의 값으로 설정
        moveSpeed = moveBox;
        if (patrolTarget1 != null)
        {
            // patrolTarget1으로 향하는 방향을 계산하고, 이 방향으로 이동
            Vector3 direction = (patrolTarget1.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.LookAt(patrolTarget1);


            // 플레이어가 공격 범위 내에 있고 현재 공격 중이 아닌 경우
            if (Time.time - lastTimePlayerInRange < attackGracePeriod && !isShooting)
            {
                // Patrol 상태에서 Gun 상태로 전환
                Debug.Log("상태 전환: Patrol -> Attack");
                _animator.SetTrigger("PatrolToAttack");
                _teampleMonsterState = TeampleMonsterState.Attack;
            }
            // patrolTarget1에 도착한 경우
            if (Vector3.Distance(transform.position, patrolTarget1.position) < 0.1f)
            {
                // 다음 순찰 지점인 patrolTarget2로 전환
                Debug.Log("상태 전환: Patrol -> Comeback");
                _animator.SetTrigger("PatrolToComeback");
                _teampleMonsterState = TeampleMonsterState.Comeback;
            }
        }
        if (health <= 0)
        {
            Debug.Log("상태 전환: Patrol -> Die");
            _animator.SetTrigger($"Die{Random.Range(0, 1)}");
            _teampleMonsterState = TeampleMonsterState.Die;
        }

    }
    private void Attack()
    {
        moveSpeed = 0;
        float distance = Vector3.Distance(player.transform.position, transform.position);  // 플레이어와 몬스터 간 거리 계산
        transform.LookAt(player.transform.position);

        // 플레이어가 공격 범위 내에 있고 현재 공격 중이 아닌 경우
        if (distance <= attackRange && Time.time - lastTimePlayerInRange < attackGracePeriod && !isShooting)
        {
            StartCoroutine(ShootAtPlayerCoroutine());
            lastTimePlayerInRange = Time.time; // 플레이어가 감지된 최근 시간 업데이트
        }


        // 감지 범위에서 적이 벗어나면
        if (distance > attackRange && Time.time - lastTimePlayerInRange > attackGracePeriod && !isShooting)
        {
            Debug.Log("상태 전환: attack -> Patrol");
            _animator.SetTrigger("AttackToPatrol");
            _teampleMonsterState = TeampleMonsterState.Patrol;
        }
        if (health <= 0)
        {
            Debug.Log("상태 전환: Patrol -> Die");
            _animator.SetTrigger($"Die{Random.Range(0, 1)}");
            _teampleMonsterState = TeampleMonsterState.Die;
        }
    }
    public IEnumerator StrongAttackCoroutine()
    {
        var collider = currentWeapon.GetComponentInChildren<BoxCollider>();
        collider.enabled = true;
        _animator.SetTrigger("AttackToPatrol");
        yield return new WaitForSeconds(1.15f);
        GameObject swordaura = Instantiate(swordauraprefab, strongAttackpoint.position, strongAttackpoint.rotation);
        Rigidbody rb = swordaura.GetComponent<Rigidbody>();
        rb.velocity = strongAttackpoint.rotation * Vector3.forward * 20f;

        yield return new WaitForSeconds(0.55f);

        collider.enabled = false;
    }

    private void Comeback()
    {
        moveSpeed = moveBox;
        if (patrolTarget2 != null)
        {
            Vector3 direction = (patrolTarget2.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.LookAt(patrolTarget2);

            if (Vector3.Distance(transform.position, patrolTarget2.position) < 0.1f)// patrolTarget2에 도착하면
            {
                Debug.Log("상태 전환: Comeback-> Patrol");
                _animator.SetTrigger("ComebackToPatrol");
                _teampleMonsterState = TeampleMonsterState.Patrol;
            }
            if (Time.time - lastTimePlayerInRange < attackGracePeriod && !isShooting)// 감지범위에 적이 나타나면
            {
                Debug.Log("상태 전환: Comeback -> Attack");
                _animator.SetTrigger("ComebackToAttack");
                _teampleMonsterState = TeampleMonsterState.Attack;
            }
        }
        if (health <= 0)
        {
            Debug.Log("상태 전환: Patrol -> Die");
            _animator.SetTrigger($"Die{Random.Range(0, 1)}");
            _teampleMonsterState = TeampleMonsterState.Die;
        }
    }
    private void Die()
    {
        DieTimer -= Time.deltaTime;
        if (DieTimer < 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        {
            float damageAmount = 0;

            if (other.gameObject.CompareTag("basicweapon") && Time.time >= lastAttackTime + attackDelay)
            {
                healthBarUI.SetActive(true);
                Debug.Log("기본무기맞음");
                damageAmount = PlayerStat.Instance.str;

                health -= damageAmount;
                StartCoroutine(ShowDamageCoroutine(damageAmount));

                lastAttackTime = Time.time;
            }
            if (other.gameObject.CompareTag("bullet"))
            {
                healthBarUI.SetActive(true);
                PlayerBullet playerbullet = other.gameObject.GetComponent<PlayerBullet>();
                if (playerbullet.playerbullettype == PlayerBullet.PlayerBulletType.DroneBullet)
                {
                    damageAmount = PlayerStat.Instance.dronestr;
                    health -= damageAmount;
                    StartCoroutine(ShowDamageCoroutine(damageAmount));


                }
                else if (playerbullet.playerbullettype == PlayerBullet.PlayerBulletType.StrongBullet)
                {
                    damageAmount = PlayerStat.Instance.dronestr * 3;
                    health -= damageAmount;
                    StartCoroutine(ShowDamageCoroutine(damageAmount));
                }

            }
        }
    }

    private IEnumerator ShootAtPlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isShooting = false;

    }
    IEnumerator ShootAtPlayerCoroutine()
    {
        isShooting = true;
        Vector3 direction = (player.transform.position - firePoint.position).normalized;

        GameObject monsterbullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = monsterbullet.GetComponent<Rigidbody>();

        rb.velocity = direction * 10f;

        monsterbullet.transform.rotation = Quaternion.LookRotation(direction);



        yield return new WaitForSeconds(fireRate);
        isShooting = false;
    }
    void InitializeHealthBar()
    {
        healthBarSlider.maxValue = health;
        healthBarSlider.value = health;
    }
    void UpdateHealthBar()
    {
        healthBarSlider.value = health;
    }
    IEnumerator ShowDamageCoroutine(float damageAmount)
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < 30)
        {
            damage.fontSize = 36; // 기본 크기
        }

        else if (distance < 40)
        {
            damage.fontSize = 45;
        }
        else if (distance < 50)
        {
            damage.fontSize = 54;
        }
        else if (distance < 60)
        {
            damage.fontSize = 63;
        }
        else
        {
            damage.fontSize = 72;
        }
        damage.text = $"{damageAmount}";
        damage.gameObject.SetActive(true);

        yield return new WaitForSeconds(1);

        damage.gameObject.SetActive(false);
    }
}

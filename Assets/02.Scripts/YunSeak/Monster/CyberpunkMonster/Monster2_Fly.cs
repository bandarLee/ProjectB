using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.UIElements.Experimental;

public enum Monster2_FlyState
{
    Idle,
    Sensing,
    FireAttack,
    Damaged,
    Die
}
public class Monster2_Fly : MonoBehaviour
{
    [Range(0, 3)]
    public int Health;
    public int MaxHealth = 3;
    public Slider HealthSliderUI;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private Rigidbody _rigidbody;
    public float AttackDistance = 2f; 

    private Vector3 _knockbackStartPosition;
    private Vector3 _knockbackEndPosition;
    private const float KNOCKBACK_DURATION = 0.1f;
    private float _knockbackProgress = 0f;
    public float KnockbackPower = 1.2f;

    private Transform _target;         // 플레이어
    public float FindDistance = 5f;  // 감지 거리
    public Vector3 StartPosition;     // 시작 위치

    private const float IDLE_DURATION = 3f;
    private float _idleTimer;
    public Transform PatrolTarget;
    [SerializeField] ParticleSystem BoomEffect = null;




    private Monster2_FlyState _monster2_FlyState = Monster2_FlyState.Idle;
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
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
    // Update is called once per frame
    void Update()
    {
        Debug.Log(Health);
        if (Health <= 0)
        {
            Destroy(this.gameObject);
        }

        switch (_monster2_FlyState)
        {
            case Monster2_FlyState.Idle:
                Idle();
                break;

            case Monster2_FlyState.Sensing:
                Sensing();
                break;

            case Monster2_FlyState.FireAttack:
                FireAttack();
                break;

            case Monster2_FlyState.Damaged:
                Damaged();
                break;

            case Monster2_FlyState.Die:
                Die();
                break;
        }
    }

    private void Idle()
    {
        _idleTimer += Time.deltaTime;
        if(PatrolTarget != null && _idleTimer >= IDLE_DURATION)
        {
            _idleTimer = 0f;
            _animator.SetTrigger("IdleToSensing");
            Debug.Log("상태전환: Idle -> Sensing");
            _monster2_FlyState = Monster2_FlyState.Sensing;
        }
    }
    
    private void Sensing()
    {
        Vector3 dir = _target.transform.position - this.transform.position;
        dir.y = 0;
        dir.Normalize();
        _navMeshAgent.stoppingDistance = AttackDistance;
        transform.forward = dir;
    }
    private void FireAttack()
    {
        if (Vector3.Distance(_target.position, transform.position) > AttackDistance)
        {
            Debug.Log("상태 전환: FireAttack -> Sensing");
            _animator.SetTrigger("FireAttackToSensing");
            _monster2_FlyState = Monster2_FlyState.Sensing;
            return;
        }
    }

    private void Damaged()
    {
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
            _monster2_FlyState = Monster2_FlyState.Sensing;
        }
    }

    public void Hit(DamageInfo damage)
    {
        if (_monster2_FlyState == Monster2_FlyState.Die)
        {
            return;
        }

        // Todo.데미지 타입이 크리티컬이면  피흘리기
        if (damage.DamageType == DamageType.Critical)
        {
            // 전기 이펙트 코드 작성
        }



        Health -= damage.Amount;
        if (Health <= 0)
        {
            Debug.Log("상태 전환: Any -> Die");

            _animator.SetTrigger($"Die{Random.Range(1, 3)}");
            _monster2_FlyState = Monster2_FlyState.Die;
        }
        else
        {
            Debug.Log("상태 전환: Any -> Damaged");

            _animator.SetTrigger("Damaged");
            _monster2_FlyState = Monster2_FlyState.Damaged;
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
        BoomEffect.Play();
        // 폭발 이펙트 추가 
        Destroy(gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedMissile : MonoBehaviour
{
    Rigidbody _rigid = null;
    Transform _target = null;

    [SerializeField] float _speed = 0f;
    float _currentSpeed = 0f;
    [SerializeField] LayerMask _layerMask = 0;
    [SerializeField] ParticleSystem _psEffect = null;

    float _searchRange = 100f;
    public float healthDamageToPlayer = 2f;

    public float SearchRange
    {
        get { return _searchRange; }
        set { _searchRange = value; }
    }

    void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        StartCoroutine(LaunchDelay());
    }

    void Update()
    {
        if (_target != null)
        {
            if (_currentSpeed <= _speed)
                _currentSpeed += _speed * Time.deltaTime;

            transform.position += transform.up * _currentSpeed * Time.deltaTime;

            Vector3 targetDirection = (_target.position - transform.position).normalized;
            transform.up = Vector3.Lerp(transform.up, targetDirection, 0.25f);
        }
    }

    IEnumerator LaunchDelay()
    {
        yield return new WaitUntil(() => _rigid.velocity.y < 0f);
        yield return new WaitForSeconds(0.1f);

        SearchTarget();
        if (_target != null)
        {
            _psEffect.Play();
        }
    }

    void SearchTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _searchRange, _layerMask);
        if (colliders.Length > 0)
        {
            _target = colliders[Random.Range(0, colliders.Length)].transform;
        }
    }

    void ApplyDamage(GameObject target)
    {
        // 대상이 Player 태그인 경우
        if (target.CompareTag("Player"))
        {
            PlayerStat health = target.GetComponent<PlayerStat>();
            // 대상의 HP를 -2 감소
            if (health != null)
            {
                health.playerhealth -= healthDamageToPlayer;              
            }
        }
        // 대상이 Monster, Wall, Ground 태그 중 하나인 경우
        else if (target.CompareTag("Monster") || target.CompareTag("Wall") || target.CompareTag("Ground"))
        {
            PlayerStat health = target.GetComponent<PlayerStat>();
            if (health != null)
            {
                _psEffect.Play();
            }
        }

        // 폭발 이펙트 재생
        _psEffect.Play();
    }

    void OnCollisionEnter(Collision collision)
    {
        // 충돌 대상에 따라 다른 동작 수행
        ApplyDamage(collision.gameObject);

        // 미사일 파괴
        Destroy(gameObject);
    }
}
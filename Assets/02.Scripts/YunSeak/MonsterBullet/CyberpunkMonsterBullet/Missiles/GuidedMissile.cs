using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedMissile : MonoBehaviour
{
    Rigidbody _rigid = null;
    Transform _tfTarget = null;

    [SerializeField] float _speed = 0f;
    float _currentSpeed = 0f;
    [SerializeField] ParticleSystem _psEffect = null;
    public float Search = 100f;
    public string playerTag = "Player"; // 플레이어 태그

    void SearchTarget()
    {
        // 플레이어 태그를 가진 오브젝트 검색
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            // 플레이어가 존재할 경우, 타겟 설정
            _tfTarget = playerObject.transform;
        }
    }

    IEnumerator LaunchDelay()
    {
        yield return new WaitUntil(() => _rigid.velocity.y < 0f);
        yield return new WaitForSeconds(0.1f);

        SearchTarget();
        _psEffect.Play();
    }

    void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        StartCoroutine(LaunchDelay());
    }

    void Update()
    {
        if (_tfTarget != null)
        {
            if (_currentSpeed <= _speed)
                _currentSpeed += _speed * Time.deltaTime;

            transform.position += transform.up * _currentSpeed * Time.deltaTime;

            Vector3 t_dir = (_tfTarget.position - transform.position).normalized;
            transform.up = Vector3.Lerp(transform.up, t_dir, 0.25f);
        }
    }

    void ApplyDamage(GameObject target)
    {
        // 폭발 이펙트 재생
        _psEffect.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌 대상에 따라 다른 동작 수행
        if (collision.gameObject.CompareTag(playerTag))
        {
            ApplyDamage(collision.gameObject);
        }

        // 미사일 파괴
        Destroy(gameObject);
    }
}
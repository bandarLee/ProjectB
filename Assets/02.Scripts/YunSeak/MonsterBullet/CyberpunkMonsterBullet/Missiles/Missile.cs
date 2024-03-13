using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    Rigidbody m_rigid = null;
    Transform m_tfTarget = null;

    [SerializeField] float m_speed = 0f;
    float m_currentSpeed = 0f;
    [SerializeField] LayerMask m_layerMask = 0;
    [SerializeField] ParticleSystem m_psEffect = null;
    public float Search = 100f;

    void SearchTarget()
    {
        Collider[] t_cols = Physics.OverlapSphere(transform.position, Search, m_layerMask);
        if (t_cols.Length > 0)
        {
            m_tfTarget = t_cols[Random.Range(0, t_cols.Length)].transform;
        }
    }

    IEnumerator LaunchDelay()
    {
        yield return new WaitUntil(() => m_rigid.velocity.y < 0f);
        yield return new WaitForSeconds(0.1f);

        SearchTarget();
        m_psEffect.Play();
    }

    void Start()
    {
        m_rigid = GetComponent<Rigidbody>();
        StartCoroutine(LaunchDelay());
    }

    void Update()
    {
        if (m_tfTarget != null)
        {
            if (m_currentSpeed <= m_speed)
                m_currentSpeed += m_speed * Time.deltaTime;

            transform.position += transform.up * m_currentSpeed * Time.deltaTime;

            Vector3 t_dir = (m_tfTarget.position - transform.position).normalized;
            transform.up = Vector3.Lerp(transform.up, t_dir, 0.25f);
        }
    }

    void ApplyDamage(GameObject target)
    {
        // 폭발 이펙트 재생
        m_psEffect.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌 대상에 따라 다른 동작 수행
        ApplyDamage(collision.gameObject);

        // 미사일 파괴
        Destroy(gameObject);
    }
}

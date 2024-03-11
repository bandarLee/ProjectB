using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { A, B, C };   // ㅁ : 근거리 공격형, B:돌격 공격형, C:원거리 공격형
    public Type enemyType;
    public int maxHealth;
    public int curHealth;
    public float rotationSpeed = 500f;

    public Transform target;        // 쫒아가기 목표
    public CapsuleCollider meleeArea;
    public GameObject bullet;

    public bool isChase;            // 추적 중
    public bool isAttack;            // 공격 중

    Rigidbody rigid;
    CapsuleCollider capsleCollider;
    NavMeshAgent nav;
    Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        capsleCollider = GetComponent<CapsuleCollider>();
        //mat = GetComponent<MeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        Invoke("ChaseStart", 2);
    }

    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);

    }

    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        if (isChase)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }

    }

    void Targeting()
    {
        float targetRadius = 0f;
        float targetRange = 0f;

        switch (enemyType)
        {
            case Type.A:
                targetRadius = 1.5f;
                targetRange = 3f;
                break;
            case Type.B:
                targetRadius = 1f;
                targetRange = 12f;
                break;
            case Type.C:
                targetRadius = 0.5f;
                targetRange = 25f;
                break;
        }

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position,
                                                                    targetRadius,                         // 구체 반지름
                                                                    transform.forward,                             // 나가는 방향
                                                                    targetRange,                        // 거리
                                                                    LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && !isAttack)
        {
            StartCoroutine(Attack());
        }
        //foreach (RaycastHit hitObject in rayHits)
        //{
        //    // 수류탄 범위 내의 피격 대상의 피격 함수 호출
        //    hitObject.transform.GetComponent<Enemy>().HitByGrenade(transform.position);
        //}


    }

    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        anim.SetBool("isAttack", true);

        switch (enemyType)
        {
            case Type.A:
                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1f);
                break;
            case Type.B:
                yield return new WaitForSeconds(0.1f);
                // 돌격
                rigid.AddForce(transform.forward * 20, ForceMode.Impulse);
                meleeArea.enabled = true;
                // 정지
                yield return new WaitForSeconds(0.5f);
                rigid.velocity = Vector3.zero;
                meleeArea.enabled = false;

                yield return new WaitForSeconds(2f);
                break;
            case Type.C:
                // 미사일 발사 준비
                yield return new WaitForSeconds(0.5f);
                GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);
                Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
                rigidBullet.velocity = transform.forward * 20;
                yield return new WaitForSeconds(2f);
                break;
        }


        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
    }

    // 물리관련 업데이트 구현
    private void FixedUpdate()
    {
        Targeting();
        FreezeVelocity();
    }

    void FreezeVelocity()
    {
        // 물리력이 NavAgent의 이동을 방해하지 않도록 
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.tag == "Melee")
    //        Debug.Log("Melee Attack -- Enemy");
    //}


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;

            Vector3 reactVec = transform.position - other.transform.position;


            //Debug.Log("Melee attack ( curHealth : " + curHealth.ToString() + " )");


        }
        else if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;

            Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject);
            //Debug.Log("Bullet attack ( curHealth : " + curHealth.ToString() + " )");

        }
    }

    public void HitByGrenade(Vector3 explosionPos)
    {
        curHealth -= 100;
        Vector3 reactVec = transform.position - explosionPos;

    }

 
    
}

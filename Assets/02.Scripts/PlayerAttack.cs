using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator playerAnimator;
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    private bool isAttacking = false;
    public GameObject swordweapon;
    public bool doubleAttack = false;
    public GameObject swordauraprefab;
    public Transform strongAttackpoint;


    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        swordweapon.GetComponentInChildren<BoxCollider>().enabled = false;

    }

    void Update()
    {
        if (Input.GetMouseButton(0) && !isAttacking)
        {
            StartCoroutine(AttackCoroutine());
        }
        if (Input.GetMouseButton(1) && !isAttacking)
        {
            StartCoroutine(StrongAttackCoroutine());

        }

    }
    public IEnumerator AttackCoroutine()
    {
        swordweapon.GetComponentInChildren<BoxCollider>().enabled = true;

        isAttacking = true;
        playerAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.15f); 
        if (Input.GetMouseButton(0))
        {
            doubleAttack = true;
        }
        playerAnimator.SetBool("DoubleAttack", doubleAttack); 

        yield return new WaitForSeconds(1f);
        if (doubleAttack)
        {
         
            doubleAttack = false; 
        }
        swordweapon.GetComponentInChildren<BoxCollider>().enabled = false;

        isAttacking = false;
        playerAnimator.SetBool("DoubleAttack", doubleAttack);

    }
    public IEnumerator StrongAttackCoroutine()
    {
        swordweapon.GetComponentInChildren<BoxCollider>().enabled = true;

        isAttacking = true;
        playerAnimator.SetTrigger("StrongAttack");
        yield return new WaitForSeconds(1.15f);
        GameObject swordaura = Instantiate(swordauraprefab, strongAttackpoint.position, strongAttackpoint.rotation);
        Rigidbody rb = swordaura.GetComponent<Rigidbody>();
        rb.velocity = strongAttackpoint.rotation * Vector3.forward * 20f;


        yield return new WaitForSeconds(0.55f);

        swordweapon.GetComponentInChildren<BoxCollider>().enabled = false;

        isAttacking = false;

    }
}

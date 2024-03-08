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
    
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
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
        yield return new WaitForSeconds(0.15f); // 첫 번째 공격이 진행되는 동안 더블 어택 입력을 기다림
        if (Input.GetMouseButton(0)) // 더블 어택 입력 감지
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
       
        swordweapon.GetComponentInChildren<BoxCollider>().enabled = false;

        isAttacking = false;

    }
}

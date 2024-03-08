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

    }
    public IEnumerator AttackCoroutine()
    {
        swordweapon.GetComponentInChildren<BoxCollider>().enabled = true;

        isAttacking = true;
        playerAnimator.SetTrigger("Attack");

        yield return new WaitForSeconds(1.15f);
        swordweapon.GetComponentInChildren<BoxCollider>().enabled = false;

        isAttacking = false;


    }
}

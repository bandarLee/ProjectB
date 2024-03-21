using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PlayerAttack : MonoBehaviour
{
    private static PlayerAttack m_instance;

    private Animator playerAnimator;
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    private bool isAttacking = false;
    public GameObject[] weapons; // 무기 오브젝트 배열
    public int currentWeaponIndex = 0; 
    public bool doubleAttack = false;
    public GameObject swordauraprefab;
    public Transform strongAttackpoint;
    public static PlayerAttack Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<PlayerAttack>();
            }

            return m_instance;
        }
    }
    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        var currentWeapon = weapons[currentWeaponIndex];
        var collider = currentWeapon.GetComponentInChildren<BoxCollider>();
        collider.enabled = false;
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
        var currentWeapon = weapons[currentWeaponIndex];
        var collider = currentWeapon.GetComponentInChildren<BoxCollider>();
        collider.enabled = true;
        isAttacking = true;
        playerAnimator.SetTrigger("Attack");
        PlayerAudioManager.instance.PlayAudio(5);
        yield return new WaitForSeconds(0.15f); 
        if (Input.GetMouseButton(0))
        {
            PlayerAudioManager.instance.PlayAudio(5);// 검소리수정필요
            doubleAttack = true;
        }
        playerAnimator.SetBool("DoubleAttack", doubleAttack); 

        yield return new WaitForSeconds(1f);
        if (doubleAttack)
        {
            PlayerAudioManager.instance.PlayAudio(6);
            doubleAttack = false; 
        }
        collider.enabled = false;

        isAttacking = false;
        playerAnimator.SetBool("DoubleAttack", doubleAttack);

    }
    public IEnumerator StrongAttackCoroutine()
    {
        var currentWeapon = weapons[currentWeaponIndex];
        var collider = currentWeapon.GetComponentInChildren<BoxCollider>();
        collider.enabled = true;
        isAttacking = true;
        playerAnimator.SetTrigger("StrongAttack");
        PlayerAudioManager.instance.PlayAudio(7);// 검기 소리 수정필요
        yield return new WaitForSeconds(1.15f);
        GameObject swordaura = Instantiate(swordauraprefab, strongAttackpoint.position, strongAttackpoint.rotation);
        Rigidbody rb = swordaura.GetComponent<Rigidbody>();
        rb.velocity = strongAttackpoint.rotation * Vector3.forward * 20f;


        yield return new WaitForSeconds(0.55f);

        collider.enabled = false;
        isAttacking = false;

    }
    public void ChangeWeapon(int weaponIndex)
    {
        // 모든 무기를 비활성화
        foreach (var weapon in weapons)
        {
            weapon.SetActive(false);
        }

        // 선택된 무기만 활성화
        weapons[weaponIndex].SetActive(true);

        currentWeaponIndex = weaponIndex; // 현재 무기 인덱스 업데이트
        var currentWeapon = weapons[currentWeaponIndex];
        var collider = currentWeapon.GetComponentInChildren<BoxCollider>();
        collider.enabled = false;

    }
}

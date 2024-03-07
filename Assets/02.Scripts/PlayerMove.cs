using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Animator playerAnimator; 
    private PlayerInput playerInput; 
    private Rigidbody playerRigidbody;
    private bool isJumping = false;
    public float jumpForce = 5f;
    public float moveSpeed = 5f;
    public float rotateSpeed = 90f;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Rotate(); // 마우스 입력에 따른 회전도 Update에서 처리.
        playerAnimator.SetFloat("Move", Mathf.Abs(playerInput.move) + Mathf.Abs(playerInput.rotate));
        if (Input.GetKey(KeyCode.Space) && !isJumping)
        {
            StartCoroutine(JumpCoroutine());
        }
    }

    private void Move()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(hAxis, 0, vAxis).normalized;
        Vector3 moveDir = transform.forward * vAxis + transform.right * hAxis;
        moveDir = moveDir.normalized * moveSpeed * Time.deltaTime;


        playerRigidbody.MovePosition(playerRigidbody.position + moveDir);
    }

    private void Rotate()
    {
        float turn = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        transform.Rotate(0f, turn, 0f);
    }
    public IEnumerator JumpCoroutine()
    {
        isJumping = true;
        playerAnimator.SetTrigger("Jump");
        playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        yield return new WaitForSeconds(1.4f);
        isJumping = false;


    }

}

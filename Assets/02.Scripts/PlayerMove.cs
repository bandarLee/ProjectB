using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Animator playerAnimator; 
    private PlayerInput playerInput; 
    private Rigidbody playerRigidbody;
    private bool isJumping = false;
    private bool isRunning = false;

    public float doublePressTime = 0.3f;
    public float jumpForce = 5f;
    public float moveSpeed = 5f;
    public float rotateSpeed = 90f;
    private float lastKeyPressTime = -1f;
    private KeyCode lastKeyCode = KeyCode.None;
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        CheckDoublePress(KeyCode.W);
        CheckDoublePress(KeyCode.A);
        CheckDoublePress(KeyCode.S);
        CheckDoublePress(KeyCode.D);

        if (isRunning)
        {
            moveSpeed = 10f; 
        }
        else
        {
            moveSpeed = 5f;
        }

        Move();
        Rotate(); // 마우스 입력에 따른 회전도 Update에서 처리.
        playerAnimator.SetFloat("Move", Mathf.Abs(playerInput.move));
        playerAnimator.SetFloat("MoveSide", Mathf.Abs(playerInput.moveside));

        if (Input.GetKey(KeyCode.Space) && !isJumping)
        {
            StartCoroutine(JumpCoroutine());
        }
    }

    private void CheckDoublePress(KeyCode keyCode)
    {
        if (Input.GetKeyDown(keyCode))
        {
            if (Time.time - lastKeyPressTime < doublePressTime && lastKeyCode == keyCode)
            {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }
            lastKeyPressTime = Time.time;
            lastKeyCode = keyCode;
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

        yield return new WaitForSeconds(1.2f);
        isJumping = false;


    }

}

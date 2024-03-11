using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PlayerMove : MonoBehaviour
{

    private Animator playerAnimator;
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    private bool isJumping = false;
    private bool isRunning = false;
    private static PlayerMove m_instance;

    public float doublePressTime = 0.3f;
    public float jumpForce = 5f;
    public float moveSpeed = 5f;
    public float rotateSpeed = 90f;
    private float lastKeyPressTime = -1f;
    private KeyCode lastKeyCode = KeyCode.None;
    private bool isFlying = false;
    private bool isAttacking = false;
    private bool sideMove = false;
    public  bool isPositionFixed = false;
    
    public static PlayerMove instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<PlayerMove>();
            }

            return m_instance;
        }
    }
    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

    }


    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isPositionFixed)
        {
            CheckDoublePress(KeyCode.W);
            CheckDoublePress(KeyCode.A);
            CheckDoublePress(KeyCode.S);
            CheckDoublePress(KeyCode.D);
            if (!isFlying)
            {
                Rotate();
            }

            //SideMove();
        }
    }

    
    void FixedUpdate()
    {
        if (!isPositionFixed)
        {
            if (isFlying)
            {
                Rotate();
            }

            if (isRunning)
            {
                playerAnimator.SetBool("IsRunning", true);

                moveSpeed = 10f;
            }
            else
            {
                playerAnimator.SetBool("IsRunning", false);

                moveSpeed = 5f;
            }

            Move();
            playerAnimator.SetFloat("Move", Mathf.Abs(playerInput.move));
            playerAnimator.SetFloat("MoveSide", playerInput.moveside);

            if (Input.GetKey(KeyCode.Space) && !isJumping)
            {
                StartCoroutine(JumpCoroutine());
            }
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
    private void SideMove()
    {
        if (Mathf.Abs(playerInput.moveside) > 0.2)
        {
            sideMove = true;
            playerAnimator.SetBool("SideMove", true);

        }
        else
        {
            sideMove = false;
            playerAnimator.SetBool("SideMove", false);

        }
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


    yield return new WaitForSeconds(0.05f);

    float timeInAir = 0f;
    bool continueFlying = false; 

    while (!isFlying && timeInAir < 1.2f)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isFlying = true;
            playerAnimator.SetBool("IsFlying", true);
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, jumpForce / 2, playerRigidbody.velocity.z);
            continueFlying = true;
            break; 
        }
        yield return null;
        timeInAir += Time.deltaTime;
    }

    while (continueFlying && Input.GetKey(KeyCode.Space))
    {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, jumpForce , playerRigidbody.velocity.z);
            yield return null;
    }

    isJumping = false;
    isFlying = false;
    playerAnimator.SetBool("IsFlying", false);
}


}

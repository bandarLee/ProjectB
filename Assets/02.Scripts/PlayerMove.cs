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
    public bool isJumping = false;
    private bool isRunning = false;
    private static PlayerMove m_instance;

    public float doublePressTime = 0.3f;
    public float jumpForce = 5f;
    public float moveSpeed = 5f;
    public float rotateSpeed = 90f;
    private float lastKeyPressTime = -1f;
    private KeyCode lastKeyCode = KeyCode.None;
    public bool isFlying = false;
    private bool isAttacking = false;
    private bool sideMove = false;
    public  bool isPositionFixed = false;

    public GameObject wsmoke;
    public GameObject fsmoke;
    
    private AudioSource audioSource; // AudioSource 변수 추가

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
        fsmoke.SetActive(false);
        
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
            if (isJumping && Input.GetKeyDown(KeyCode.Space) )
            {
                Debug.Log("비행 소리 재생");
                PlayerAudioManager.instance.StopSpecificAudio(3);
                PlayerAudioManager.instance.StopSpecificAudio(2);
                PlayerAudioManager.instance.PlayAudio(0);
             

            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                PlayerAudioManager.instance.StopSpecificAudio(1);
            }
        }

    }

    
    void FixedUpdate()
    {
        if (!isPositionFixed)
        {
            if (isFlying)
            {
                wsmoke.SetActive(false);
                fsmoke.SetActive(true);
                
                Rotate();
            }

            if (isRunning)
            {
                playerAnimator.SetBool("IsRunning", true);

                moveSpeed = 8f;
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
            if (isFlying && Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                Debug.Log("비행 소리 재생");
                PlayerAudioManager.instance.StopSpecificAudio(3);
                PlayerAudioManager.instance.PlayAudio(2, true);
                PlayerAudioManager.instance.PlayAudio(1);
            }

        }
        if(PlayerInput.instance.move == 0 && PlayerInput.instance.moveside == 0)
        {
            PlayerAudioManager.instance.StopSpecificAudio(3);
            PlayerAudioManager.instance.StopSpecificAudio(2);
        }
    }

    private void CheckDoublePress(KeyCode keyCode)
    {
        if (Input.GetKeyDown(keyCode))
        {
            if (Time.time - lastKeyPressTime < doublePressTime && lastKeyCode == keyCode)
            {
                PlayerAudioManager.instance.StopSpecificAudio(2);

                Debug.Log("더블이동사운드 체크");
                PlayerAudioManager.instance.PlayAudio(3,true);
                // 오디오 사운드 Loop 이동시활성화true 작성 필요 
                isRunning = true;
            }
            else
            {
                PlayerAudioManager.instance.StopSpecificAudio(3);

                Debug.Log("이동사운드 체크");
                PlayerAudioManager.instance.PlayAudio(2,true);
                // 오디오 사운드 Loop 이동시활성화 이동불가시 비활성화 필요
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
        moveDir = moveDir.normalized * moveSpeed * Time.deltaTime * PlayerStat.Instance.speed;


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
    public void StopAudio()
    {
        audioSource.Stop(); // AudioSource의 재생 중인 오디오를 중지
    }

    public IEnumerator JumpCoroutine()
    {

        isJumping = true;
        playerAnimator.SetTrigger("Jump");
        playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        PlayerAudioManager.instance.PlayAudio(4); // 점프 음향

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
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, jumpForce, playerRigidbody.velocity.z);
            yield return null;
        }

        isJumping = false;
        isFlying = false;

        playerAnimator.SetBool("IsFlying", false);
        yield return new WaitForSeconds(2f);

        fsmoke.SetActive(false);
        wsmoke.SetActive(true);
    }
    public IEnumerator Timewaitcouroutine()
    {
        yield return new WaitForSeconds(0.5f);
    }
}

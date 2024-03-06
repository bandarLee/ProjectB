using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터
    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디

    public float speed = 5f;
    private float rotspeed = 10f;

    public float jumpheight = 3f;
    public float dash = 5f;
    public Vector3 dir = Vector3.zero;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        dir.x = Input.GetAxis("Horizontal");
        dir.y = 0;
        dir.z = Input.GetAxis("Vertical");
    

    }
    private void FixedUpdate()
    {
        if (dir != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, dir, rotspeed * Time.deltaTime);
        }

        playerRigidbody.MovePosition(this.gameObject.transform.position + dir * speed * Time.deltaTime);
    }
}

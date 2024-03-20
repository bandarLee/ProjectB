using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    public float speed = 5f; // 이동 속도
    public float stopTime = 0.1f;// 고정속도
    public float DestroedTime = 5f;
    public float FlyDestroedTime = 10f;
    private bool isColliding = false; // 충돌 여부를 나타내는 플래그

    void Update()
    {
        // 충돌 중인 경우 이동을 멈춤
        if (isColliding)
            return;

        // 이동 방향 계산 (앞쪽 방향)
        Vector3 moveDirection = transform.forward;

        // 이동 벡터에 속도를 곱해 이동량을 조절
        Vector3 moveAmount = moveDirection * speed * Time.deltaTime;

        // 현재 위치에 이동량을 더함으로써 전진
        transform.position += moveAmount;

        FlyDestroedTime -= Time.deltaTime;
        if(FlyDestroedTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    // 충돌 감지
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트에 고정을 지연시킴
        StartCoroutine(FixToAfterDelay(collision.gameObject, stopTime));
    }

    // 일정 시간 후에 다른 오브젝트에 고정하는 함수
    private IEnumerator FixToAfterDelay(GameObject otherObject, float delay)
    {
        // 충돌 후 일정 시간 대기
        yield return new WaitForSeconds(delay);

        // 충돌한 오브젝트의 부모를 변경하여 고정
        transform.parent = otherObject.transform;
        isColliding = true; // 충돌 중 플래그 설정

        // 5초 후에 게임 오브젝트 파괴
        StartCoroutine(DestroyAfterDelay(DestroedTime));
    }

    // 지연 후 게임 오브젝트 파괴
    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
    
}

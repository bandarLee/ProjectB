using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject Door;
    public float liftingSpeed = 1.0f;
    private bool isLifting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(LiftDoor());
        }
    }
    private IEnumerator LiftDoor()
    {
        if (Door != null && !isLifting)
        {
            isLifting = true;

            // Door의 현재 위치 가져오기
            Vector3 startDoorPosition = Door.transform.position;

            // 목표 위치 계산 (y 값을 5만큼 증가)
            Vector3 targetDoorPosition = startDoorPosition + Vector3.up * 5f;

            float elapsedTime = 0f;

            while (elapsedTime < 1f)
            {
                // Door를 천천히 올리기
                Door.transform.position = Vector3.Lerp(startDoorPosition, targetDoorPosition, elapsedTime);

                elapsedTime += Time.deltaTime * liftingSpeed;

                yield return null;
            }

            // Door의 위치 최종 설정
            Door.transform.position = targetDoorPosition;

            Debug.Log("Door가 천천히 올라감");

            isLifting = false;
        }
    }
}

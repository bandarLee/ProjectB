using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorLeft : MonoBehaviour
{
    public GameObject Door1;
    public GameObject Door2;
    public float liftingSpeed = 1.0f;
    private bool isLifting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(LiftDoors());
        }
    }

    private IEnumerator LiftDoors()
    {
        if (Door1 != null && Door2 != null && !isLifting)
        {
            isLifting = true;

            // Door1의 현재 위치 가져오기
            Vector3 startDoor1Position = Door1.transform.position;

            // Door1 목표 위치 계산 (z 값을 -4로 이동)
            Vector3 targetDoor1Position = new Vector3(startDoor1Position.x, startDoor1Position.y, -4f);

            // Door2의 현재 위치 가져오기
            Vector3 startDoor2Position = Door2.transform.position;

            // Door2 목표 위치 계산 (z 값을 0으로 이동)
            Vector3 targetDoor2Position = new Vector3(startDoor2Position.x, startDoor2Position.y, 0f);

            float elapsedTime = 0f;

            while (elapsedTime < 1f)
            {
                // Door1을 천천히 이동
                Door1.transform.position = Vector3.Lerp(startDoor1Position, targetDoor1Position, elapsedTime);

                // Door2를 천천히 이동
                Door2.transform.position = Vector3.Lerp(startDoor2Position, targetDoor2Position, elapsedTime);

                elapsedTime += Time.deltaTime * liftingSpeed;

                yield return null;
            }

            // Door1의 위치 최종 설정
            Door1.transform.position = targetDoor1Position;

            // Door2의 위치 최종 설정
            Door2.transform.position = targetDoor2Position;

            Debug.Log("Door1이 -4로 이동, Door2가 0으로 이동");

            isLifting = false;
        }
    }
}

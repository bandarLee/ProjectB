using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    // 따라다닐 대상의 Transform을 지정하는 변수
    public Transform Target;

    // 카메라의 Y 좌표와 대상 간의 거리
    public float YDistance = 20f;

    // 초기 오일러 각도를 저장하는 변수
    private Vector3 _initialEulerAngles;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) 
        {
            Transform targetInPlayer = player.transform.Find("MinimapTarget");
            if (targetInPlayer != null) 
            {
                Target = targetInPlayer;
            }
        }
        // 초기 오일러 각도를 현재 카메라의 각도로 설정
        _initialEulerAngles = transform.eulerAngles;
    }

    private void LateUpdate()
    {
        if (Target != null)
        {
            // 대상의 현재 위치를 가져옴
            Vector3 targetPosition = Target.position;


            // 대상과의  Y 좌표 거리를 적용하여 카메라의 목표 위치를 설정
            targetPosition.y = YDistance;


            // 카메라의 위치를 목표 위치로 설정하여 따라다니도록 함
            transform.position = targetPosition;
        }
    }
}

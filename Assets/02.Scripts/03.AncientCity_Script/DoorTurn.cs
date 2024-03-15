using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTurn : MonoBehaviour
{
    public Transform pivotPoint; // 회전할 기준점
    public float rotationSpeed = 5f; // 회전 속도
    public float targetAngle = 120f; // 목표 각도
    private float currentRotation = 0f; // 현재 회전한 각도를 추적
    public bool clockwise = false;
    public bool clear = false;

    void Update()
    {
        if (clear)
        {
            if (currentRotation < targetAngle)
            {
                float step = rotationSpeed * Time.deltaTime; // 이번 프레임에서 회전할 각도
                float rotationAmount = Mathf.Min(step, targetAngle - currentRotation); // 실제 회전할 각도
                currentRotation += rotationAmount;

                if (clockwise)
                {
                    transform.RotateAround(pivotPoint.position, -Vector3.up, rotationAmount);
                }
                else
                {
                    transform.RotateAround(pivotPoint.position, Vector3.up, rotationAmount);
                }
            }
        }
        else 
        {
            // 원하는 상황에..
            
        }
    }
    
}

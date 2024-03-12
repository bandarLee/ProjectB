using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    /*  public float mouseSensitivity = 100f; 
      public Transform playerBody; 
      private float xRotation = 0f; */
    public CinemachineVirtualCamera virtualCamera;

    void Start()
    {
/*        Cursor.lockState = CursorLockMode.Locked; 
*/    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Z)) 
        {
            SetClosestEnemyAsTarget();

        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            RemoveTarget();
        }
        void SetClosestEnemyAsTarget()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
            GameObject closestEnemy = null;
            float closestDistance = Mathf.Infinity;
            Vector3 currentPosition = transform.position;

            foreach (GameObject enemy in enemies)
            {
                Vector3 directionToTarget = enemy.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistance)
                {
                    closestDistance = dSqrToTarget;
                    closestEnemy = enemy;
                }
            }

            if (closestEnemy != null)
            {
                virtualCamera.LookAt = closestEnemy.transform;
            }
        }

        void RemoveTarget()
        {
            virtualCamera.LookAt = null;
        }
        /*        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; 
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime; 

                xRotation -= mouseY; 
                xRotation = Mathf.Clamp(xRotation, -90f, 90f); 

                transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); 
                playerBody.Rotate(Vector3.up * mouseX); */
    }
}

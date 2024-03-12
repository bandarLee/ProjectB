using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLockOn : MonoBehaviour
{
    public Transform playerBody; // 플레이어의 Transform

    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            GameObject closestEnemy = FindClosestEnemy();
            if (closestEnemy != null)
            {
                LookAtEnemy(closestEnemy.transform);
            }
        }
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = playerBody.position;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, currentPosition);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    void LookAtEnemy(Transform enemyTransform)
    {
        Vector3 direction = enemyTransform.position - playerBody.position;
        direction.y = 0; 
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        playerBody.rotation = Quaternion.Slerp(playerBody.rotation, lookRotation, Time.deltaTime * 10);
    }
}

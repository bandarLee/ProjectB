using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1BossWheelMove : MonoBehaviour
{
    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(MoveTowardsPlayer());
        Destroy(gameObject, 4.00f);

    }

    void Update()
    {
        
    }
    IEnumerator MoveTowardsPlayer()
    {
        yield return new WaitForSeconds(2f);

        float duration = 0.6f; 
        float elapsedTime = 0f; 

        Vector3 start = transform.position; 
        Vector3 end = playerTransform.position; 

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(start, end, (elapsedTime / duration));
            elapsedTime += Time.deltaTime; 
            yield return null;
        }

       
    }
}

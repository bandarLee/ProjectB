using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimContr : MonoBehaviour
{
    public float moveDistance = 5f; 
    public float moveSpeed = 2f; 
    private bool isMovingUp = true; 

    void Start()
    {
        StartCoroutine(MoveUpDownCoroutine());
    }

    public IEnumerator MoveUpDownCoroutine()
    {
        while (true) 
        {
            Vector3 startPosition = transform.position; 
            Vector3 endPosition;

            if (isMovingUp)
            {
                endPosition = startPosition + Vector3.up * moveDistance;
            }
            else
            {
                endPosition = startPosition - Vector3.up * moveDistance;
            }

            float journey = 0f;
            while (journey <= moveDistance)
            {
                journey += moveSpeed * Time.deltaTime;
                transform.position = Vector3.Lerp(startPosition, endPosition, journey / moveDistance);
                yield return null;
            }

            yield return new WaitForSeconds(1f);

            isMovingUp = !isMovingUp;
        }
    }
}

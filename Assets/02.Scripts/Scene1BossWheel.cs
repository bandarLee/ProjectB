using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Scene1BossWheel : MonoBehaviour
{
    public GameObject[] cubeArray = new GameObject[27];
    public float rotationSpeed = 0.5f;
    public float launchDelay = 2f;
    private bool isLaunched = false; 

    void Start()
    {
        StartCoroutine(RotateCubeSidesSequentially());



    }
  
    IEnumerator RotateCubeSidesSequentially()
    {
        RotateFaceX(0);
        RotateFaceX(1);

        RotateFaceX(2);
        yield return new WaitForSeconds(1f);

    }
   
  
    public GameObject GetCubeAt(int x, int y, int z)
    {
        int index = x + y * 3 + z * 9;
        return cubeArray[index];
    }

    void RotateFaceX(int x)
    {

        Vector3 pivot = GetCubeAt(x, 1, 1).transform.position;
        float angle = (x == 1) ? -90f : 90f;

        for (int y = 0; y < 3; y++)
        {
            for (int z = 0; z < 3; z++)
            {
                GameObject cube = GetCubeAt(x, y, z);
                StartCoroutine(RotateAroundPivot(cube, pivot, Vector3.right, angle)); // x축 기준으로 회전
            }
        }
    }

    IEnumerator RotateAroundPivot(GameObject cube, Vector3 pivot, Vector3 axis, float angle)
    {
        Quaternion startRotation = cube.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(axis * angle) * startRotation;
        float duration = 10f;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            cube.transform.rotation = Quaternion.Lerp(startRotation, endRotation, timeElapsed / duration);
            cube.transform.position = pivot + Quaternion.Euler(axis * (angle * (timeElapsed / duration))) * (cube.transform.position - pivot);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

    
    }
}

using System.Collections;
using UnityEngine;

public class Scene1BossCubeGround : MonoBehaviour
{
    public GameObject[] cubeArray = new GameObject[27];
    public float rotationSpeed = 0.51f;

    public GameObject bosshead;
    public bool stop = false;

    void OnEnable()
    {
    
        StartCoroutine(RotateCubeSidesSequentially());
    }
    private void Awake()
    {
     
    }
    IEnumerator RotateCubeSidesSequentially()
    {
        while (true)
        {
            StartCoroutine(RotateLayerX(0, true));
            StartCoroutine(RotateLayerX(1, false));
            StartCoroutine(RotateLayerX(2, true));



            yield return new WaitForSeconds(2f);
            StartCoroutine(RotateLayerX(0, false));
            StartCoroutine(RotateLayerX(1, true));
            StartCoroutine(RotateLayerX(2, false));
            yield return new WaitForSeconds(2f);

        }

    }

    IEnumerator RotateLayerX(int x, bool clockwise)
    {
        float angle = clockwise ? 90f : -90f;
        Vector3 axis = Vector3.right;

        GameObject temporaryParent = new GameObject("TemporaryParent");
        Vector3 pivot = GetCubeAt(x, 1, 1).transform.position;
        temporaryParent.transform.position = pivot;

        for (int y = 0; y < 3; y++)
        {
            for (int z = 0; z < 3; z++)
            {
                GameObject cube = GetCubeAt(x, y, z);
                cube.transform.SetParent(temporaryParent.transform);
            }
        }

        float duration = 1f / rotationSpeed;
        float timeElapsed = 0f;
        Quaternion startRotation = temporaryParent.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(axis * angle) * startRotation;

        while (timeElapsed < duration)
        {
            temporaryParent.transform.rotation = Quaternion.Lerp(startRotation, endRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        temporaryParent.transform.rotation = endRotation;

        temporaryParent.transform.rotation = startRotation;

        while (temporaryParent.transform.childCount > 0)
        {
            temporaryParent.transform.GetChild(0).SetParent(transform);
        }

        Destroy(temporaryParent);
    }

    
    public GameObject GetCubeAt(int x, int y, int z)
    {
        int index = x + y * 3 + z * 9;
        return cubeArray[index];
    }
}
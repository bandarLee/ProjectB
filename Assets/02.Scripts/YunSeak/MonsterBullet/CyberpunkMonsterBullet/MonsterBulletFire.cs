using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBulletFire : MonoBehaviour
{
    public GameObject[] prefabsToSpawn; // 프리팹 배열
    public Transform spawnPoint; // 생성 위치
    public float spawnInterval = 3f; // 생성 간격

    private void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // 랜덤한 프리팹 선택
            int randomIndex = Random.Range(0, prefabsToSpawn.Length);
            GameObject prefabToSpawn = prefabsToSpawn[randomIndex];

            // 프리팹 생성
            Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);
        }
    }
}

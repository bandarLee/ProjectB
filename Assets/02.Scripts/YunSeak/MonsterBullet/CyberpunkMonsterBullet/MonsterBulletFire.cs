using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBulletFire : MonoBehaviour
{
    public GameObject[] prefabsToSpawn; // 프리팹 배열
    public Transform spawnPoint; // 생성 위치
    public float spawnInterval = 0.1f; // 생성 간격
    private Transform _target;
    public Transform monster;
    public string playerTag = "Player"; // 플레이어 태그

    private void Start()
    {
        StartCoroutine(SpawnObjects());
        _target = transform.parent;
    }

    private void Update()
    {
        if (_target != null)
        {
            // 몬스터가 플레이어를 향해 바라보도록 합니다.
            transform.LookAt(_target);
        }

        if (spawnInterval == 0f)
        {
            spawnInterval = 1f;
        }
        return;
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
            GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPoint.position, monster.rotation);
        }
    }
}
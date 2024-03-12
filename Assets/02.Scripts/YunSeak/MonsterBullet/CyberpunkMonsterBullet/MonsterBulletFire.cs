using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBulletFire : MonoBehaviour
{
    public GameObject[] prefabsToSpawn; // 프리팹 배열
    public Transform spawnPoint; // 생성 위치
    public float spawnInterval = 3f; // 생성 간격
    private float _knockbackProgress = 0f; // 총기반동 / 넉백
    private Vector3 _knockbackStartPosition;
    private Vector3 _knockbackEndPosition;
    private Transform _target;
    public float KnockbackPower = 1.2f;
    private const float KNOCKBACK_DURATION = 0.1f;
    private Animator _animator;

    private void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    private void Update()
    {
        if (spawnInterval == 0f)
        {
            spawnInterval = 3f;
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
            GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);

            // 프리팹이 Sniper_Rifle_Magazine_Mesh 태그를 가지고 있는 경우에만 처리
            if (spawnedObject.CompareTag("Sniper_Rifle_Magazine_Mesh"))
            {
                // 프리팹에 포함된 넉백 및 폭발 이펙트 로직 실행
                //HandleKnockbackAndExplosion(spawnedObject);
            }
        }
    }

    //void HandleKnockbackAndExplosion(GameObject targetObject)
    //{
    //    // 넉백 시작 위치 설정
    //    _knockbackStartPosition = targetObject.transform.position;

    //    // 플레이어 방향으로의 단위 벡터 계산
    //    Vector3 dir = targetObject.transform.position - _target.position;
    //    dir.y = 0;
    //    dir.Normalize();

    //    // 넉백 종료 위치 설정
    //    _knockbackEndPosition = targetObject.transform.position + dir * KnockbackPower;

    //    _knockbackProgress = 0f; // 넉백 진행도 초기화

    //    // 넉백 및 폭발 이펙트 실행
    //    StartCoroutine(PerformKnockbackAndExplosion(targetObject));
    //}

    //IEnumerator PerformKnockbackAndExplosion(GameObject targetObject)
    //{
    //    while (_knockbackProgress < 1f)
    //    {
    //        // Lerp를 이용해 넉백 진행
    //        _knockbackProgress += Time.deltaTime / KNOCKBACK_DURATION;
    //        targetObject.transform.position = Vector3.Lerp(_knockbackStartPosition, _knockbackEndPosition, _knockbackProgress);

    //        yield return null;
    //    }

    //    // 폭발이펙트 작성
    //}
}
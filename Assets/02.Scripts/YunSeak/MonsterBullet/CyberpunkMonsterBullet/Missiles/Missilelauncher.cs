using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    [SerializeField] GameObject _goMissile = null;
    [SerializeField] Transform _MissileSpawn = null;
    public float UpSpeed = 50f;
    public string playerTag = "Player"; // 플레이어 태그
    private GameObject player; // 플레이어 오브젝트

    public float detectionRadius = 100f; // 플레이어 감지 반경
    public float spawnInterval = 1f; // 프리팹 출력 간격
    private float timer = 0f; // 경과 시간을 저장할 변수

    void Start()
    {
        // 플레이어 오브젝트를 찾아서 player 변수에 할당합니다.
        player = GameObject.FindGameObjectWithTag(playerTag);
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어가 존재하면서 일정 반경 안에 있을 때
        if (player != null && Vector3.Distance(transform.position, player.transform.position) < detectionRadius)
        {
            // 경과 시간을 누적합니다.
            timer += Time.deltaTime;

            // 지정된 간격마다 프리팹을 출력합니다.
            if (timer >= spawnInterval)
            {
                // 프리팹 생성 및 초기 속도 설정
                GameObject _missile = Instantiate(_goMissile, _MissileSpawn.position, Quaternion.identity);
                _missile.GetComponent<Rigidbody>().velocity = Vector3.up * UpSpeed;

                // 경과 시간 초기화
                timer = 0f;
            }
        }
    }
}
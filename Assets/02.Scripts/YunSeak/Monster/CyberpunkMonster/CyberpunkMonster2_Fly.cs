using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CyberpunkMonster2_Fly : MonoBehaviour
{
    [Range(0, 1)]
    public int Health;
    public int MaxHealth = 1;
    public Slider HealthSliderUI;
    public string playerTag = "Player"; // 플레이어 태그
    public float detectionRange = 2f; // 감지범위
    private GameObject player; // 플레이어 오브젝트
    [SerializeField] ParticleSystem BoomEffect = null;
    public GameObject explosionPrefab;  // 폭발 이펙트 프리팹
    void Start()
    {
        // 플레이어 오브젝트를 찾아서 player 변수에 할당합니다.
        player = GameObject.FindGameObjectWithTag(playerTag);
    }

    void Update()
    {
        // 플레이어가 존재하면
        if (player != null)
        {
            // 플레이어와 몬스터의 거리를 계산합니다.
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // 플레이어와의 거리가 작동 거리보다 짧으면
            if (distanceToPlayer <= detectionRange)
            {
                // 몬스터가 플레이어를 향해 바라보도록 합니다.
                transform.LookAt(player.transform);
            }
        }

        if (Health <= 0)
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        // 1초 뒤 폭발 필요성?

        // 1. 폭발 이펙트 생성
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        // 2. BoomEffect 재생
        BoomEffect.Play();
        Destroy(gameObject);
    }
}
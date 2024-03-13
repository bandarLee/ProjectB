using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleLookAtTarget : MonoBehaviour
{
    private GameObject monster; // 몬스터 오브젝트

    void Start()
    {
        // 몬스터 오브젝트를 찾아서 monster 변수에 할당합니다.
        monster = transform.parent.gameObject;
    }

    void Update()
    {
        if (monster != null)
        {
            // 몬스터가 바라보는 방향과 동일하게 Muzzle도 회전합니다.
            transform.rotation = monster.transform.rotation;
        }
    }
}
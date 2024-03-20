using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TempleBulletType
{
    Health,  // 체력을 줄인다
    GuidedMissile, // 스피드를 줄인다.
    Smoke,   // 연막탄
    Boom,    // 불화살
    Frozen     // 번개
}

public class TempleMonsterBullet : MonoBehaviour
{

    public TempleBulletType templeBulletType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections.Generic;
using UnityEngine;

// 아이템 공장의 역할: 아이템 오브젝트의 생성을 책임진다.
// **팩토리 패턴**
// 객체 생성을 공장 클래스를 이용해 캡슐화 처리하여 대신 "생성"하게 하는 디자인 패턴
// 객체 생성에 필요한 과정을 템플릿화 해놓고 외부에서 쉽게 사용한다.
// 장점:
// 1. 생성과 처리 로직을 분리하여 결합도를 낮출 수 있다. (결합도: 참조를 통해 상호 의존성의 높아지는 정도)
// 2. 확장 및 유지보수가 편리하다.
// 3. 객체 생성 후 공통으로 할 일을 수행하도록 지정해줄 수 있다.
// 단점:
// 1. 상대적으로 쪼끔 더 복잡하다.
// 2. 그래서 공부해야 한다.
// 3. 그거 말고는 단점이 크게 없다.

public class MonsterBulletObjectFactory : MonoBehaviour
{
    public static MonsterBulletObjectFactory Instance { get; private set; }

    // (생성할)아이템 프리팹들
    public List<GameObject> BulletPrefabs;

    //오브젝트 풀링 웅덩이라는 뜻이나 이해를 위해 공장의 창고로 표현 중
    // 공장의 창고
    private List<BulletObject> _BulletPool;
    public int PoolCount = 10;

    private void Awake()
    {
        Instance = this;

        _bulletPool = new List<BulletObject>();

        for (int i = 0; i < PoolCount; ++i)            // 10번
        {
            foreach (GameObject prefab in BulletPrefabs) // 3개
            {
                // 1. 만들고
                GameObject Bullet = Instantiate(prefab);
                // 2. 창고에 넣는다.
                Bullet.transform.SetParent(this.transform);
                _BulletPool.Add(Bullet.GetComponent<BulletObject>());
                // 3. 비활성화
                Bullet.SetActive(false);
            }
        }
    }

    private BulletObject Get(BulletType BulletType) // 창고 뒤지기
    {
        foreach (BulletObject BulletObject in _BulletPool) // 창고를 뒤진다.
        {
            if (BulletObject.gameObject.activeSelf == false
                && BulletObject.BulletType == BulletType)
            {
                return BulletObject;
            }
        }

        return null;
    }

    // 확률 생성 (공장아! 랜덤박스 주문할게!)
    public void MakePercent(Vector3 position)
    {
        int percentage = UnityEngine.Random.Range(0, 100);
        if (percentage <= 20) // 20%
        {
            Make(BulletType.Health, position);
        }
        else if (percentage <= 40)
        {
            Make(BulletType.Stamina, position);
        }
        else if (percentage <= 50)
        {
            Make(BulletType.Bullet, position);
        }
    }

    // 기본 생성 (공장아! 내가 원하는거 주문할게!)
    public void Make(BulletType BulletType, Vector3 position)
    {
        BulletObject BulletObject = Get(BulletType);

        if (BulletObject != null)
        {
            BulletObject.transform.position = position;
            BulletObject.Init();
            BulletObject.gameObject.SetActive(true);
        }
    }
}

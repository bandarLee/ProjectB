using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestImage : MonoBehaviour
{
    public Transform targetObject; // 따라다닐 대상의 Transform을 참조할 변수

    void Update()
    {
        if (targetObject != null)
        {
            transform.position = targetObject.position;
            // 필요에 따라서 이미지가 따라다니는 대상의 위치에 추가적인 조정을 할 수 있습니다.
            // 예를 들어, y축 방향으로 조정하려면 아래와 같이 합니다.
            // transform.position = new Vector3(targetObject.position.x, targetObject.position.y + yOffset, targetObject.position.z);
        }
    }
}

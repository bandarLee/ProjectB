using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveAxisName = "Vertical"; 
    public string moveSideAxisName = "Horizontal"; 
    // 값 할당은 내부에서만 가능
    public float move { get; private set; } 
    public float moveside { get; private set; } 


    // 매프레임 사용자 입력을 감지
    private void Update()
    {

        moveside = Input.GetAxis(moveSideAxisName);

        move = Input.GetAxis(moveAxisName);




    }
}
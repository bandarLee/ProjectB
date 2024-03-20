using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PlayerInput : MonoBehaviour
{
    public string moveAxisName = "Vertical"; 
    public string moveSideAxisName = "Horizontal"; 

    public float move { get; private set; } 
    public float moveside { get; private set; }
    private static PlayerInput m_instance;

    public bool IsOptionOpen = false;

    public static PlayerInput instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<PlayerInput>();
            }

            return m_instance;
        }
    }
    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
        UI_Option.Instance.gameObject.SetActive(false);


    }
    // 매프레임 사용자 입력을 감지
    private void Update()
    {

        moveside = Input.GetAxis(moveSideAxisName);

        move = Input.GetAxis(moveAxisName);

        UIManage();



    }
    private void UIManage()
    {

        if ((Input.GetKeyDown(KeyCode.Escape)) || (Input.GetKeyDown(KeyCode.M))||(Input.GetKeyDown(KeyCode.I)) && !IsOptionOpen)
        {

            IsOptionOpen = !IsOptionOpen;
            if (IsOptionOpen)
            {               
                UI_Option.Instance.Open();
                //PlayerAudioManager.instance.StopSpecificAudio(12); // 플레이어 UI 시작 사운드
            }
            else
            {              
                UI_Option.Instance.Close();
                //PlayerAudioManager.instance.StopSpecificAudio(13);// 플레이어 UI 종료 사운드
            }

        }
    }
}
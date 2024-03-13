using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2GameManager : MonoBehaviour
{
    public static Scene2GameManager instance { get; private set; }

    public UI_Option OptionUI;

    private Coroutine _blinkingCoroutine;

    public GameObject MinimapNPCPosition;

    private void Awake()
    {
        instance = this;
        MinimapNPCPosition.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }
    public void Continue()
    {
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnOptionButtonClicked();
        }

    }

    public void OnOptionButtonClicked()
    {
        Pause();
        OptionUI.Open();
    }

    // 미니맵에서 NPC 위치를 깜빡깜빡 거리며 나타냄
    private IEnumerator BlinkingEffect()
    {
        MinimapNPCPosition.SetActive(true);
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            // 활성화 되어 있으면 비활성화로, 비활성화 되어 있으면 활성화로 ( 깜빡이는 효과 )
            MinimapNPCPosition.SetActive(!MinimapNPCPosition.activeSelf);
        }
    }

    public void StopBlinking()
    {
        StopCoroutine(BlinkingEffect());
        Destroy(MinimapNPCPosition);
    }
    public void StartBlinking()
    {
        _blinkingCoroutine = StartCoroutine(BlinkingEffect());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTest : MonoBehaviour
{
    public float fadeDuration = 1.0f; // 페이드 아웃 지속 시간
    public SpriteRenderer spriteRenderer; // 스프라이트 렌더러
    private Color originalColor; // 초기 색상
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        
    }

    void Update()
    {
        if (Scene2GameManager.instance._isCandle1 == true && Scene2GameManager.instance._isCandle2 == true && Scene2GameManager.instance._isCandle3 == true) 
        {
            StartFadeOut();
        }
    }
    public void StartFadeOut() 
    {
        StartCoroutine(FadeOut());
    }
    IEnumerator FadeOut()
    {
        // 페이드 아웃 반복
        float timer = 0f;
        while (timer < fadeDuration)
        {
            // 투명도 감소
            float alpha = Mathf.Lerp(originalColor.a, 0f, timer / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            // 시간 업데이트
            timer += Time.deltaTime;

            // 다음 프레임까지 대기
            yield return null;
        }

        // 페이드 아웃 완료 후 개체 비활성화 또는 다른 동작 수행
        gameObject.SetActive(false);
    }
}

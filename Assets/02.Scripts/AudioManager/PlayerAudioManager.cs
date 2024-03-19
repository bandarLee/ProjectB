using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    private bool isPlaying = false; // 사운드 재생 중인지 여부를 추적하는 변수

    public static PlayerAudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlayAudio(int index)
    {
        if (!isPlaying && index < audioClips.Length) // 재생 중이 아니고 인덱스가 유효한 경우에만 재생
        {
            StartCoroutine(PlaySoundCoroutine(index)); // 코루틴으로 사운드 재생 시작
        }
    }
    IEnumerator PlaySoundCoroutine(int index)
    {
        isPlaying = true; // 재생 중으로 설정

        audioSource.clip = audioClips[index]; // 클립 설정
        audioSource.Play(); // 재생

        yield return new WaitForSeconds(audioClips[index].length); // 사운드 길이만큼 대기

        isPlaying = false; // 재생 종료 후 재생 중 상태 해제
    }
 
}
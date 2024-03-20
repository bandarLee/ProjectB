using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySceneAudioManager : MonoBehaviour
{
    public AudioSource[] audioSources;
    public AudioClip[] audioClips;
    // 오디오 사운드 Loop 이동시활성화 이동불가시 비활성화 필요
    private bool isPlaying = false; // 사운드 재생 중인지 여부를 추적하는 변수
    // private bool isLoop = false;// 사운드 루프 재생 여부 추적하는 변수
    public static LobbySceneAudioManager instance;

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
        audioSources = GetComponents<AudioSource>();

    }

    public void PlayAudio(int clipIndex, bool loop = false)
    {
        if (clipIndex < audioClips.Length)
        {
            foreach (AudioSource audioSource in audioSources)
            {
                if (audioSource.clip == audioClips[clipIndex] && audioSource.isPlaying)
                {
                    audioSource.Play();
                    return;
                }
            }

            foreach (AudioSource audioSource in audioSources)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = audioClips[clipIndex];
                    audioSource.loop = loop;

                    audioSource.Play();
                    return;
                }
            }
        }
    }
    public void StopSpecificAudio(int clipIndex)
    {
        if (clipIndex < audioClips.Length)
        {
            foreach (var audioSource in audioSources)
            {
                if (audioSource.clip == audioClips[clipIndex])
                {
                    audioSource.Stop();
                    break;
                }
            }
        }
    }
}

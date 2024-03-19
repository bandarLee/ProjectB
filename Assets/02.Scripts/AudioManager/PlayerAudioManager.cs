using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public AudioSource[] audioSources;
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
        audioSources = GetComponents<AudioSource>();

    }

    public void PlayAudio(int clipIndex)
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
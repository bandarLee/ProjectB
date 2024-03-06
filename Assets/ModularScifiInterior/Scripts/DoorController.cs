using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    private Animator _doorAnim;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private float openSoundDelay;
    [SerializeField] private AudioClip closeSound;
    [SerializeField] private float closeSoundDelay;

    private void OnTriggerEnter(Collider other)
    {
        _doorAnim.SetBool("isOpening", true);
        Invoke(nameof(PlayOpeningSound), openSoundDelay);
    }

    private void OnTriggerExit(Collider other)
    {
        _doorAnim.SetBool("isOpening", false);
        Invoke(nameof(PlayClosingSound), closeSoundDelay);
    }

    // Start is called before the first frame update
    void Awake()
    {
        _doorAnim = this.transform.parent.GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayOpeningSound()
    {
        _audioSource.PlayOneShot(openSound);
    }

    private void PlayClosingSound()
    {
        _audioSource.PlayOneShot(closeSound);
    }
}

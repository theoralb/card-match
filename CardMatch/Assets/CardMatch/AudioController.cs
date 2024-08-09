using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip FlipSound;
    public AudioClip MatchSound;
    public AudioClip WrongSound;
    public AudioClip EndSound;
    private AudioSource audioSource;
    // Start is called before the first frame update
    public void Start()
    {
        audioSource = GetComponent<AudioSource>();  
    }

    public void PlayEffect(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }
}

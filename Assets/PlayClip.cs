using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayClip : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip laugh;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    public void Play()
    {
        audioSource.PlayOneShot(laugh);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioPicker : MonoBehaviour
{
    public AudioClip [] clips;
   
    void Start()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.clip = clips[(int)Random.Range(0,clips.Length)];
        source.Play();
        Debug.Log("Audio Clip: " + source.clip.name);
    }

  
}

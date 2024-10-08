using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPickerRandomStart : MonoBehaviour
{
    public AudioClip [] clips;
   
    void Start()
    {
        AudioSource source = this.GetComponent<AudioSource>();
        source.clip = clips[(int)Random.Range(0,clips.Length)];
        source.time = Random.Range(0f, source.clip.length);
        source.Play();
        
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


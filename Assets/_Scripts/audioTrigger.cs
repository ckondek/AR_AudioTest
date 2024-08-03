using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioTrigger : MonoBehaviour
{
    AudioSource _AudioSource;

    void Start()
    {
         _AudioSource = GetComponent<AudioSource>();
    }


   void OnTriggerEnter(Collider collidingThing)
    {
        if (collidingThing.gameObject.tag == "MainCamera")
        {
           _AudioSource.Play();
        }

        
    }

}


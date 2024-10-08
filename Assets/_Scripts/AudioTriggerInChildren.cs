using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioTriggerInChildren : MonoBehaviour
{
    AudioSource _AudioSource;
    

    void Start()
    {
         _AudioSource = GetComponentInChildren<AudioSource>();
    }


   void OnTriggerEnter(Collider collidingThing)
    {
        if (collidingThing.gameObject.tag == "MainCamera")
        {
           _AudioSource.Play();
        }

        
    }
   
}



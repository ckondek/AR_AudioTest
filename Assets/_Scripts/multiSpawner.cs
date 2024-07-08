using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class multiSpawner : MonoBehaviour
{
   

    public GameObject [] objects;
    public int amount;



     void Start()
    {
        for (int x = 0; x < amount; x++)
        {
            int index = (int)Random.Range(0, objects.Length);
            Instantiate(objects[index], new Vector3(0,0,0), Quaternion.identity);
           
        }

    }
        
    
    

}




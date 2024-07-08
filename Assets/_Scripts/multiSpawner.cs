using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class multiSpawner : MonoBehaviour
{
   

    public GameObject [] objects;
    public int amount;
    public int xPosition;
    public int yPosition;
    public int zPosition;



     void Start()
    {
        for (int x = 0; x < amount; x++)
        {
            int index = (int)Random.Range(0, objects.Length);
            Instantiate(objects[index], new Vector3(xPosition,yPosition,zPosition), Quaternion.identity);
           
        }

    }
        
    
    

}




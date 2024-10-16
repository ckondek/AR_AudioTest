using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class multiSpawner : MonoBehaviour
{
   

    public GameObject [] objects;
    public int amount;
    public float xStart;
    public float xEnd;
    public float yStart;
    public float yEnd;
    public float zStart;
    public float zEnd;

    private List<GameObject> spawnedList = new List<GameObject>();
   



     void Start()
    {
        for (int x = 0; x < amount; x++)
        {
            int index = (int)Random.Range(0, objects.Length);
            GameObject obj = Instantiate(objects[index], new Vector3(Random.Range(xStart, xEnd),Random.Range(yStart, yEnd), Random.Range(zStart, zEnd)), Quaternion.identity);
            spawnedList.Add(obj);
           
        }

    }
        
    
    

}




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{

    public GameObject spawnObject;


    public int rows = 5;
    public int columns = 5;
    public float spread = 0.50f;

    
    public bool random = false;
    public int amount = 20;
    public int rangeX_cm = 40;
    public int rangeZ_cm =  40;

    

    // Start is called before the first frame update
    void Start()
    {
        if (random)
        {
            for (int n = 0; n < amount; n++)
            {
                float x = Random.Range(-rangeX_cm, rangeX_cm);
                float z = Random.Range(100,rangeZ_cm);
                Instantiate(spawnObject, new Vector3(x/100,0,z/100), Quaternion.identity);  
            }
               
        } else

        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {

                    Instantiate(spawnObject, new Vector3(r * spread,0, c * spread), Quaternion.identity); 
                   
                }
            }
        }
    }

   
}



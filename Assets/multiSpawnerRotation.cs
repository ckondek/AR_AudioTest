using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class multiSpawnerRotation : MonoBehaviour
{
    public GameObject[] objects;
    public int amount;
    public float xStart;
    public float xEnd;
    public float yStart;
    public float yEnd;
    public float zStart;
    public float zEnd;

    void Start()
    {
        for (int x = 0; x < amount; x++)
        {
            int index = (int)Random.Range(0, objects.Length);

            // Position zufällig generieren
            Vector3 spawnPosition = new Vector3(Random.Range(xStart, xEnd), Random.Range(yStart, yEnd), Random.Range(zStart, zEnd));
            
            // Rotation des Prefabs übernehmen
            Quaternion spawnRotation = objects[index].transform.rotation;
            
            // Objekt instanziieren mit Originalrotation
            Instantiate(objects[index], spawnPosition, spawnRotation);
        }
    }
}




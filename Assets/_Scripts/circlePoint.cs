using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circlePoint : MonoBehaviour
{
    public GameObject rotationCenter;
    public float radius = 2f;
    public float speed = 1f;
    public float angle =0f; 

    void Update()
    {
       
       float x = rotationCenter.transform.position.x * Mathf.Sin(angle) * radius;
       float y = rotationCenter.transform.position.y;
       float z = rotationCenter.transform.position.z * Mathf.Cos(angle) * radius; 
       angle += speed * Time.deltaTime;

       transform.position = new Vector3(x,y,z);
    }
}

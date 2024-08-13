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
       
       float x = rotationCenter.transform.position.x + Mathf.Cos(angle) * radius;
       float y = rotationCenter.transform.position.y + Mathf.Sin(angle) * 0.3f;
       float z = rotationCenter.transform.position.z + Mathf.Sin(angle) * radius; 
       angle += speed * Time.deltaTime;

       this.transform.position = new Vector3(x,y,z);
    }
}

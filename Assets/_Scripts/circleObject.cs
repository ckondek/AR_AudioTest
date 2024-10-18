using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circleObject : MonoBehaviour
{
   
    public float radius = 2f;
    public float speed = 1f;
    public float angle =0f; 
    public float xOffset = 0f;
    public float yOffset = 0f;
    public float zOffset = 0f;
    Vector3 camOffset;
     GameObject cam;

    void Start(){


        cam = GameObject.FindGameObjectWithTag("MainCamera");
        camOffset = new Vector3(xOffset,yOffset,zOffset);

    }


    void Update()
    {

       Vector3 origin = cam.transform.position;
        origin = origin + camOffset;
       float x = origin.x + Mathf.Cos(angle) * radius;
       float y = origin.y + Mathf.Sin(angle) * 0.3f;
       float z = origin.z + Mathf.Sin(angle) * radius; 
       angle += speed * Time.deltaTime;

       this.transform.position = new Vector3(x,y,z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lissajousLines : MonoBehaviour
{
    public int resolution;
    [SerializeField, Range(0, 200)] float freqX;
    [SerializeField, Range(0, 200)] float freqY;
     [SerializeField, Range(0, 200)] float freqZ;
    [SerializeField, Range(0, 200)] float ampX;
    [SerializeField, Range(0, 200)] float ampY;
    [SerializeField, Range(0, 200)] float ampZ;
    [SerializeField, Range(0, 200)] float phaseX;
    [SerializeField, Range(0, 200)] float freq2X;
    [SerializeField, Range(-2, 2)]  float offsetX;
    [SerializeField, Range(-2, 2)]  float offsetY;
    [SerializeField, Range(-1, 5)]  float offsetZ;
    private float x;
    private float y;
    private float z;
    private Vector3[] points;

    private LineRenderer _lineRenderer;

    private static float mapSpecial(int value, float fromLow, float fromHigh, float toLow, float toHigh) 
    {
        return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
    }   


    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
         _lineRenderer.positionCount = resolution;
        points = new Vector3[resolution];
       
    }

    void Update(){

         for (int p =0; p < resolution; p++){

            float angle = mapSpecial(p,0,resolution, 0, Mathf.PI * 2);
            x = Mathf.Sin( angle * freqX  + phaseX ) * ampX;
            x = x * Mathf.Sin(angle * freq2X);
            y = Mathf.Sin( angle * freqY) * ampY;
            z = Mathf.Sin( angle * freqZ) * ampZ;
            _lineRenderer.SetPosition(p, new Vector3(x + offsetX,y + offsetY ,z + offsetZ));

        }


    }   

        

}




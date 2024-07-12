using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridMaker : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 20;
        for (int i = 0; i < 10; i++)


        {
            int index = i * 2;
            _lineRenderer.SetPosition(index, new Vector3(-1,0,i));
            _lineRenderer.SetPosition(index + 1, new Vector3(1,0,i));
        }
    }

}

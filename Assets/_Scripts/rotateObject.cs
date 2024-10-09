using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateObject : MonoBehaviour
{
    // Start is called before the first frame update
     // Geschwindigkeit der Rotation
    public Vector3 rotationSpeed = new Vector3(0, 100, 0); // Rotation um die y-Achse mit 100 Grad pro Sekunde

    void Update()
    {
        // Drehung des Objekts pro Frame
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}

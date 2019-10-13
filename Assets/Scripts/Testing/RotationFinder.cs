using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFinder : MonoBehaviour
{

    public Vector3 pointA;
    public Vector3 pointB;

    // Start is called before the first frame update
    void Start()
    {                
        Debug.Log(Quaternion.FromToRotation(pointA - pointB, Vector3.down).eulerAngles);

        Debug.Log(Vector3.Distance(pointA, pointB));

        Debug.Log((pointA + pointB)/2.0f);
    }

}

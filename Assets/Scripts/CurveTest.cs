using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;

public class CurveTest : MonoBehaviour
{
    public Vector3 A;
    public Vector3 B;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Vector3 v in WireHelper.CurvedPoints(A, B))
        {
            Debug.Log(v);
        }
    }
}

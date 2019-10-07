using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mono_Toggle : MonoBehaviour
{
    public void FlipSwitch(bool value)
    {
        Transform child = gameObject.transform.Find("0");
        Vector3 v = child.position;
        v.y = value ? -0.5f : 0.508f;
        child.position = v;
    }
}

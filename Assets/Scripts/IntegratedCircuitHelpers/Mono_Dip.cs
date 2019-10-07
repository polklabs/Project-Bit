using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mono_Dip : MonoBehaviour
{
    public void FlipSwitch(int index, bool value)
    {
        Transform child = gameObject.transform.Find(index.ToString());
        child.rotation = Quaternion.Euler(0, value ? 180 : 0, 0);
    }
}

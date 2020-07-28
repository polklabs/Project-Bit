using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralManager : MonoBehaviour
{
    public string workSpaceName;

    public bool paused = false;
    public int step = 0;
   
    void Start()
    {
        workSpaceName = PlayerPrefs.GetString("Workspace");
        Application.targetFrameRate = 60;
    }
}

using System.Runtime.Serialization;
using UnityEngine;

public struct GeneralManagerStruct
{
    public bool paused;
    public int step;
    public Vector3 position;
}

public class GeneralManager : MonoBehaviour
{
    public string workSpaceName;

    public bool paused = false;
    public int step = 0;

    public Transform gameCamera;
   
    void Start()
    {
        workSpaceName = PlayerPrefs.GetString("Workspace");
        Application.targetFrameRate = 60;
    }

    public GeneralManagerStruct GenerateStruct()
    {
        GeneralManagerStruct gms = new GeneralManagerStruct();
        gms.paused = paused;
        gms.step = step;
        gms.position = gameCamera.position;
        return gms;
    }

    public void CopyStruct(GeneralManagerStruct gms)
    {
        paused = gms.paused;
        step = gms.step;
        gameCamera.position = gms.position;
    }
}

using System.Runtime.Serialization;
using UnityEngine;

public struct GeneralManagerStruct
{
    public bool paused;
    public int step;
    public int singleStep;
    public Vector3 position;
    public bool showLabels;
}

public class GeneralManager : Singleton<GeneralManager>
{
    public string workSpaceName;

    public bool paused = false;
    public int step = 0;
    public int singleStep = 0;
    public bool showLabels = false;

    public Transform gameCamera;

    private BreadBoard breadBoard;

    new void Awake()
    {
        breadBoard = BreadBoard.GetInstance();
    }
   
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
        gms.singleStep = singleStep;
        gms.position = gameCamera.position;
        gms.showLabels = showLabels;
        return gms;
    }

    public void CopyStruct(GeneralManagerStruct gms)
    {
        PlayPause(gms.paused, true);
        step = gms.step;
        singleStep = gms.singleStep;
        gameCamera.position = gms.position;
        showLabels = gms.showLabels;
    }

    public void PlayPause(bool value = false, bool useValue = false)
    {
        if (useValue)
        {
            paused = value;
        }
        else
        {
            paused = !paused;
        }

        if (paused)
        {
            breadBoard.SetNodeDisplay();
            breadBoard.SetComponentDisplay();
        } else
        {
            breadBoard.ClearNodeDisplay();
            breadBoard.ClearComponentDisplay();
        }
    }

    public void Step(int inc = 2)
    {
        if (paused)
        {
            step += inc;
        }
    }

    public void DecrementStep()
    {
        if (step > 0)
        {
            step--;
        }
    }

    public void SingleStep()
    {
        if (paused)
        {
            singleStep++;
        }
    }

    public void DecrementSingleStep()
    {
        if (singleStep > 0)
        {
            singleStep--;
        }
    }

    public void ToggleLabels()
    {
        breadBoard.SetLabelDisplays();
    }
}

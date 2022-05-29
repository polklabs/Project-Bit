using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CanvasType
{
    MainMenu,
    GameUI,
    PauseMenu
}

public class CanvasManager : Singleton<CanvasManager>
{
    List<CanvasController> canvasControllers;
    CanvasController lastActiveCanvas;
    
    new void Awake()
    {
        canvasControllers = GetComponentsInChildren<CanvasController>().ToList();
        canvasControllers.ForEach(x => x.gameObject.SetActive(false));
        SwitchCanvas(CanvasType.MainMenu);
    }

    public void SwitchCanvas(CanvasType type)
    {
        if (lastActiveCanvas != null)
        {
            lastActiveCanvas.gameObject.SetActive(false);
        }

        CanvasController canvasController = canvasControllers.Find(x => x.canvasType == type);
        if (canvasController != null)
        {
            canvasController.gameObject.SetActive(true);
            lastActiveCanvas = canvasController;
        } else
        {
            Debug.LogWarning("The desired canvas was not found!");
        }
    }
}

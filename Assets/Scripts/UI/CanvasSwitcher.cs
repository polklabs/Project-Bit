using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CanvasSwitcher : MonoBehaviour
{
    public CanvasType desiredCanvasType;

    CanvasManager canvasManager;
    Button menuButton;

    void Start()
    {
        canvasManager = CanvasManager.GetInstance();
        menuButton = GetComponent<Button>();
        menuButton.onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        canvasManager.SwitchCanvas(desiredCanvasType);
    }
}

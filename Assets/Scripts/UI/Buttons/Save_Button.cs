using UnityEngine;
using UnityEngine.UI;

public class Save_Button : MonoBehaviour
{
    private Save saveManager;
    private Button button;

    void Start()
    {
        saveManager = Save.GetInstance();
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(SaveWorkspace);
    }

    void SaveWorkspace()
    {
        saveManager.SaveWorkspaceGUI();
    }
}

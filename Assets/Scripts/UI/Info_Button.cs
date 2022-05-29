using UnityEngine;
using UnityEngine.UI;

public class Info_Button : MonoBehaviour
{
    private UI_DataSheet ui_DataSheet;
    private Button button;

    private string dataSheetName;
    void Start()
    {
        ui_DataSheet = UI_DataSheet.GetInstance();
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(ShowDataSheet);
    }

    public void SetDataSheet(string className)
    {
        dataSheetName = className;
        if (!DataSheet.HasDataSheet(className))
        {
            gameObject.SetActive(false);
        }
    }
    
    void ShowDataSheet()
    {
        if (dataSheetName != null)
        {
            ui_DataSheet.ShowDataSheet(dataSheetName);
        }
    }
}

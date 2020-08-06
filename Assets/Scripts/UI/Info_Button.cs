using UnityEngine;
using UnityEngine.UI;

public class Info_Button : MonoBehaviour
{
    public UI_DataSheet ui_DataSheet;
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(ShowDataSheet);
    }
    
    void ShowDataSheet()
    {
        ui_DataSheet.ShowDataSheet(gameObject.transform.parent.name);        
    }
}

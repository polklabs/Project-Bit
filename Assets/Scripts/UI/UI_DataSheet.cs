using UnityEngine;
using UnityEngine.UI;

public class UI_DataSheet : MonoBehaviour
{
    public GameObject root;

    public GameObject container;
    public GameObject imageTemplate;

    public Text title;
    public Text text;

    private string currentClassName;

    public void ShowDataSheet(string className)
    {
        if (className == currentClassName)
        {
            EnableGameObject(root, false);
            return;
        }

        DataSheet dataSheet = GetDataSheet(className);
        if(dataSheet == null)
        {
            EnableGameObject(root, false);
            return;
        }

        EnableGameObject(root, true);
        EnableGameObject(container, dataSheet.DiagramLayers.Length > 0);

        for (int i = 1; i < container.transform.childCount; i++)
        {
            Destroy(container.transform.GetChild(i).gameObject);
        }

        foreach (string layer in dataSheet.DiagramLayers)
        {
            GameObject go = Instantiate(imageTemplate) as GameObject;
            go.SetActive(true);
            go.transform.SetParent(container.transform, false);
            go.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Textures/data-sheets/" + layer);
        }

        title.text = dataSheet.PartName;
        text.text = dataSheet.Text;

        currentClassName = className;
    }

    DataSheet GetDataSheet(string className)
    {
        if(DataSheet.DataSheetList.ContainsKey(className))
        {
            return DataSheet.DataSheetList[className];
        }
        return null;
    }

    void EnableGameObject(GameObject gameObject, bool enabled)
    {
        gameObject.SetActive(enabled);
    }

}

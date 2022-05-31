using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewWorkspaceManager : MonoBehaviour
{
    public InputField input;
    public Button createButton;

    // Start is called before the first frame update
    void Start()
    {
        createButton.onClick.AddListener(CreateWorkspace);   
    }

    // Update is called once per frame
    void CreateWorkspace()
    {
        PlayerPrefs.SetString("Workspace", input.text);
        Fabricator.GetInstance().RemoveWorkspace();
        Load.GetInstance().LoadWorkspace(input.text);
    }
}

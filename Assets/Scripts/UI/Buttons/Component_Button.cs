using UnityEngine;
using UnityEngine.UI;
using userInterface;

public class Component_Button : MonoBehaviour
{
    private WorkspaceEditor workspaceEditor;
    private Button button;
    
    public Part part;

    // Start is called before the first frame update
    void Start()
    {
        workspaceEditor = WorkspaceEditor.GetInstance();
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(AttachIC);
    }

    public void SetPart(Part p)
    {
        part = p;
    }

    // Update is called once per frame
    void AttachIC()
    {
        workspaceEditor.SetActionPlace(true);
        workspaceEditor.AttachIC(part);
    }
}

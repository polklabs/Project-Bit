using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Component_Button : MonoBehaviour
{
    public WorkspaceEditor sm;
    private Button button;

    public bool isMultiPoint = false;

    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(AttachIC);
    }

    // Update is called once per frame
    void AttachIC()
    {
        sm.AttachIC(gameObject.name);
        //if (isMultiPoint) {
        //    sm.SetMultipoint();
        //}
    }
}

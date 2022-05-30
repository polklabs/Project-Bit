using IntegratedCircuits;
using UnityEngine;
using UnityEngine.UI;

namespace userInterface {
    public class ContextMenuManager : Singleton<ContextMenuManager>
    {

        WorkspaceEditor workspaceEditor;
        RaycastManager raycastManager;

        public Transform ContextMenuPanel;
        public GameObject ContextMenuButtonTemplate;

        private RaycastHit raycastHit;
        private bool menuOpen = false;

        new void Awake()
        {
            // We can't get a reference to a disabled object so call this
            // to make sure the instance is determined before setting active to false
            ContextMenuManager.GetInstance();

            workspaceEditor = WorkspaceEditor.GetInstance();
            raycastManager = RaycastManager.GetInstance();
            gameObject.SetActive(false);
        }

        public void OpenContextMenu()
        {
            ClearContextMenu();
            if (raycastManager.GetIntegratedCircuit(out raycastHit, out IntegratedCircuit ic))
            {
                gameObject.SetActive(true);
                DrawButtons("Integrated Circuit", true, Context_None);
                DrawButtons("Edit", !ic.isEditable, Context_Edit);
                DrawButtons("Show Labels", ic.PinNames.Length <= 0, Context_Labels);
                DrawButtons("Remove", false, Context_Remove);
                menuOpen = true;
            }
            else if (raycastManager.GetBreadBoard(out raycastHit, out string breadBoardId, out string[] _))
            {
                gameObject.SetActive(true);
                DrawButtons("Bread Board", true, Context_None);
                DrawButtons("Move", true, Context_None);
                DrawButtons("Remove", false, Context_Remove);
                menuOpen = true;
            } 
            else
            {
                CloseContextMenu();
            }
        }

        private void Update()
        {
            // Keep context menu in fixed position
            if (menuOpen)
            {
                Vector3 pos = Camera.main.WorldToScreenPoint(raycastHit.point);
                gameObject.transform.position = new Vector3(pos.x, pos.y, 0);
            }
        }

        private void DrawButtons(string name, bool disabled, UnityEngine.Events.UnityAction call)
        { 
            GameObject g = Instantiate(ContextMenuButtonTemplate, ContextMenuPanel);
            g.GetComponentInChildren<Text>().text = name;
            Button button = g.GetComponent<Button>();
            button.onClick.AddListener(call);
            button.onClick.AddListener(CloseContextMenu);
            button.interactable = !disabled;
        }

        public void CloseContextMenu()
        {
            ClearContextMenu();
            gameObject.SetActive(false);
            menuOpen = false;
        }

        private void ClearContextMenu()
        {
            for (int i = 0; i < ContextMenuPanel.childCount; i++)
            {
                Destroy(ContextMenuPanel.GetChild(i).gameObject);
            }
        }

        private void Context_None()
        {
            // Do nothing
        }

        private void Context_Remove()
        {
            workspaceEditor.RemoveAction(raycastHit);
        }

        private void Context_Edit()
        {

        }

        private void Context_Labels()
        {

        }
    }
}
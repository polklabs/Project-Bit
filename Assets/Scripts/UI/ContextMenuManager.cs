using IntegratedCircuits;
using UnityEngine;
using UnityEngine.UI;

namespace userInterface {
    public class ContextMenuManager : Singleton<ContextMenuManager>
    {

        WorkspaceEditor workspaceEditor;
        RaycastManager raycastManager;
        BreadBoard breadBoard;

        public Transform ContextMenuPanel;
        public GameObject ContextMenuButtonTemplate;

        private RaycastHit raycastHit;
        private IntegratedCircuit integratedCircuit;
        private bool menuOpen = false;

        new void Awake()
        {
            // We can't get a reference to a disabled object so call this
            // to make sure the instance is determined before setting active to false
            ContextMenuManager.GetInstance();

            workspaceEditor = WorkspaceEditor.GetInstance();
            raycastManager = RaycastManager.GetInstance();
            breadBoard = BreadBoard.GetInstance();
            gameObject.SetActive(false);
        }

        public void OpenContextMenu()
        {
            ClearContextMenu();
            UpdateMenuPosition();
            gameObject.SetActive(true);
            if (raycastManager.GetIntegratedCircuit(out raycastHit, out integratedCircuit))
            {
                
                DrawButtons("Integrated Circuit", true, Context_None);
                DrawButtons("Edit", !integratedCircuit.isEditable, Context_Edit);
                if(integratedCircuit is Switch)
                {
                    DrawButtons("Custom Label", false, Context_Edit_Label);
                }
                DrawButtons("Toggle Pinouts", integratedCircuit.PinNames.Length <= 0, Context_Labels);
                DrawButtons("Remove", false, Context_Remove);
                DrawButtons("Close", false, CloseContextMenu);
                menuOpen = true;
            }
            else if (raycastManager.GetBreadBoard(out raycastHit, out string breadBoardId, out string[] _))
            {
                DrawButtons("Bread Board", true, Context_None);
                DrawButtons("Move", true, Context_None);
                DrawButtons("Remove", false, Context_Remove);
                DrawButtons("Close", false, CloseContextMenu);
                menuOpen = true;
            } 
            else
            {
                CloseContextMenu();
            }
        }

        private void Update()
        {
            if (menuOpen)
            {
                UpdateMenuPosition();
            }
        }

        /// <summary>
        /// Keep context menu in fixed position
        /// </summary>
        private void UpdateMenuPosition()
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(raycastHit.point);
            gameObject.transform.position = new Vector3(pos.x, pos.y, 0);
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
            if (integratedCircuit != null)
            {
                breadBoard.SetLabelDisplay(integratedCircuit.GetId());
            }
        }

        private void Context_Edit_Label()
        {
            if (integratedCircuit != null)
            {
                Switch ic = (Switch)integratedCircuit;
                ic.Label = "Write";
                breadBoard.SetSwitchLabelDisplay(integratedCircuit.GetId());
            }
        }
    }
}
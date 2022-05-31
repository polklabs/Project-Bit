using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace userInterface
{
    /// <summary>
    /// This Manager handles and directs input from keyboard and mouse
    /// </summary>
    class KeyboardMouseManager : Singleton<KeyboardMouseManager>
    {
        private GeneralManager generalManager;
        private WorkspaceEditor workspaceEditor;
        private CanvasManager canvasManager;
        private ContextMenuManager contextMenuManager;

        new void Awake()
        {
            generalManager = GeneralManager.GetInstance();
            workspaceEditor = WorkspaceEditor.GetInstance();
            canvasManager = CanvasManager.GetInstance();
            contextMenuManager = ContextMenuManager.GetInstance();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                generalManager.PlayPause();
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                generalManager.SingleStep();
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                generalManager.Step();
            }

            if (Input.GetKeyDown(KeyCode.Period))
            {
                BreadBoard.GetInstance().ResetNodeValues();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (workspaceEditor.Action != WorkspaceAction.None)
                {
                    if (workspaceEditor.locationASelected)
                    {
                        workspaceEditor.ResetWire();
                    } else
                    {
                        workspaceEditor.SetDesiredAction(WorkspaceAction.None);
                    }
                } else
                {
                    canvasManager.SwitchCanvas(CanvasType.MainMenu);
                }
                
            }

            workspaceEditor.HoverAction();

            if (Input.GetMouseButtonDown(0))
            {
                workspaceEditor.ClickAction();
            }

            if (Input.GetMouseButtonDown(1))
            {
                workspaceEditor.SecondaryClickAction();
                if (workspaceEditor.Action == WorkspaceAction.None)
                {
                    contextMenuManager.OpenContextMenu();
                }
            }
        }
    }
}

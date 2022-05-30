using System;
using UnityEngine;
using UnityEngine.EventSystems;
using IntegratedCircuits;

namespace userInterface
{
    class RaycastManager : Singleton<RaycastManager>
    {
        BreadBoard breadBoard;

        new void Awake()
        {
            breadBoard = BreadBoard.GetInstance();
        }

        public bool GetIntegratedCircuit(out RaycastHit hit, out IntegratedCircuit ic)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            ic = null;

            int layer_mask = LayerMask.GetMask("Component");
            if (Physics.Raycast(ray, out hit, 500, layer_mask) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (Guid.TryParse(hit.transform.parent.gameObject.name, out Guid guid))
                {
                    ic = breadBoard.components[guid];
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public bool GetBreadBoard(out RaycastHit hit, out string breadBoardId, out string[] breadBoardChildIds)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            breadBoardId = null;
            breadBoardChildIds = null;

            int layer_mask = LayerMask.GetMask("Breadboard");
            if (Physics.Raycast(ray, out hit, 500, layer_mask) && !EventSystem.current.IsPointerOverGameObject())
            {
                breadBoardId = hit.transform.name;
                breadBoardChildIds = new string[hit.transform.childCount];
                for (int i = 0; i < hit.transform.childCount; i++)
                {
                    breadBoardChildIds[i] = hit.transform.GetChild(i).name;
                }
                return true;
            }
            return false;
        }

        public bool GetNodeId(out RaycastHit hit, out string nodeId, params string[] layerNames)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            nodeId = null;

            int layer_mask = LayerMask.GetMask(layerNames);
            if (Physics.Raycast(ray, out hit, 500, layer_mask) && !EventSystem.current.IsPointerOverGameObject())
            {
                nodeId = hit.transform.gameObject.name;
                return true;
            }
            return false;
        }
    }
}

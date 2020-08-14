using UnityEngine.EventSystems;
using UnityEngine;
using IntegratedCircuits;
using System.Linq;
using Helper;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public enum WorkspaceAction { Place, Remove, Demolish, Probe, None };

public class WorkspaceEditor : MonoBehaviour
{
    //Referances
    public GeneralManager gm;
    public BreadBoard breadBoard;
    public CircuitPool circuitPool;
    public Fabricator fabricator;
    public Mono_Switch mono_Switch;
    public Oscilloscope oscilloscope;

    public Material outlineMat;

    public WorkspaceAction Action;

    private readonly Vector3 GameObjectPool = new Vector3(0, 500, 0);

    private GameObject hoverObject = null;
    private List<Material[]> hoverMaterials = new List<Material[]>();

    /* For Placing Objects */
    private GameObject icModel = null;
    private string icName = "";
    private IntegratedCircuit ic = null;
    private bool locationASelected = false;
    private string nodeIdA = "";
    private int indexA = 0;
    private List<Vector3> wireNodes = new List<Vector3>();
    private bool showsError = false;
    private bool placeRotated = false;

    /* For Probing */
    public Text ProbeText;
    public GameObject ProbeObj;
    public GameObject HighLight;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gm.PlayPause();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            gm.SingleStep();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            gm.Step();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (locationASelected)
            {
                ResetWire();
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }

        switch (Action)
        {
            case WorkspaceAction.Place:
                PlaceAction_Hover();
                break;
            case WorkspaceAction.Probe:
                ProbeAction_Hover();
                break;
            case WorkspaceAction.Remove:
                RemoveAction_Hover();
                break;
            case WorkspaceAction.Demolish:
                DemolishAction_Hover();
                break;
            default:
                break;
        }

        if (Input.GetMouseButtonDown(0))
        {
            switch (Action)
            {
                case WorkspaceAction.Place:
                    PlaceAction();
                    break;
                case WorkspaceAction.Probe:
                    ProbeAction();
                    break;
                case WorkspaceAction.Remove:
                    RemoveAction();
                    break;
                case WorkspaceAction.Demolish:
                    DemolishAction();
                    break;
                default:
                    NoneAction();
                    break;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            switch (Action)
            {
                case WorkspaceAction.Place:
                    if (ic.IcType == ICType.wire)
                    {
                        PlaceActionWireNode();
                    } else if (
                        ic.IcType == ICType.ic4 ||
                        ic.IcType == ICType.ic6 ||
                        ic.IcType == ICType.breadboard ||
                        ic.IcType == ICType.powerrail)
                    {
                        placeRotated = !placeRotated;
                    }
                    break;                
                default:
                    break;
            }
        }
    }

    public void SetActionRemove()
    {
        ClearIC();
        if (Action == WorkspaceAction.Remove)
        {
            Action = WorkspaceAction.None;
            return;
        }
        Action = WorkspaceAction.Remove;
    }

    public void SetActionDemolish()
    {
        ClearIC();        
        if (Action == WorkspaceAction.Demolish)
        {
            Action = WorkspaceAction.None;
            return;
        }
        Action = WorkspaceAction.Demolish;
    }

    public void SetActionProbe()
    {
        ClearIC();
        if (Action == WorkspaceAction.Probe)
        {
            ProbeObj.gameObject.SetActive(false);
            Action = WorkspaceAction.None;
            return;
        }
        ProbeObj.gameObject.SetActive(true);
        Action = WorkspaceAction.Probe;
    }

    public void SetActionNone()
    {
        ClearIC();
        Action = WorkspaceAction.None;
    }

    public void SetActionPlace(bool set = false)
    {
        ClearIC();
        if (Action == WorkspaceAction.Place && !set)
        {
            Action = WorkspaceAction.None;
            return;
        }
        Action = WorkspaceAction.Place;
    }

    private void PlaceAction()
    {
        if (ic != null && (ic.IcType == ICType.breadboard || ic.IcType == ICType.powerrail))
        {
            PlaceActionBreadBoard();
            return;
        }

        int layer_mask = LayerMask.GetMask("Pin");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 500, layer_mask) && !EventSystem.current.IsPointerOverGameObject())
        {           
            if (ic != null && icModel != null)
            {

                string nodeId = hit.transform.gameObject.name;
                int index = CircuitHelper.GetIndexFromNode(nodeId, hit.point);

                int x = Mathf.RoundToInt(hit.point.x);
                int z = Mathf.RoundToInt(hit.point.z);
                icModel.transform.position = new Vector3(x, 0, z);

                if (circuitPool.CanPlace(ic.IcType, ic.Pins, nodeId, index))
                {
                    PlaceComponent(nodeId, index, new Vector3(x, 0, z));
                }
                else
                {
                    Debug.Log("Cannot place item here");
                }
            }

        }
        else if (icModel != null && ic.IcType != ICType.wire)
        {
            icModel.transform.position = GameObjectPool;
        }
    }

    private void PlaceActionBreadBoard()
    {
        int layer_mask = LayerMask.GetMask("Ground");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 500, layer_mask) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (ic != null && icModel != null)
            {

                string nodeId = hit.transform.gameObject.name;
                int index = CircuitHelper.GetIndexFromNode(nodeId, hit.point);

                int x = Mathf.RoundToInt(hit.point.x);
                int z = Mathf.RoundToInt(hit.point.z);
                icModel.transform.position = new Vector3(x, 0, z);

                string type = (ic.IcType == ICType.breadboard ? "0" : "1");

                if (fabricator.canPlaceBreadBoard(type, x, z, placeRotated))
                {
                    fabricator.addBoard(type, x, z, placeRotated);
                }
                else
                {
                    Debug.Log("Cannot place item here");
                }
            }

        }
        else
        {
            icModel.transform.position = GameObjectPool;
        }
    }

    private void PlaceActionWireNode()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 point = hit.point;
            point.y = Mathf.Clamp(point.y, 0.25f, 100f);
            wireNodes.Add(point);
            icModel.GetComponentInChildren<LineRenderer>().positionCount = wireNodes.Count;
            icModel.GetComponentInChildren<LineRenderer>().SetPositions(wireNodes.ToArray());
        }
    }

    private void PlaceActionBreadboard_Hover()
    {
        int layer_mask = LayerMask.GetMask("Ground");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 500, layer_mask) && !EventSystem.current.IsPointerOverGameObject())
        {
            int x = Mathf.RoundToInt(hit.point.x);
            int z = Mathf.RoundToInt(hit.point.z);

            if (ic != null && icModel != null)
            {

                Vector3 location = new Vector3(x, 0, z);
                icModel.transform.position = location;
                icModel.transform.rotation = Quaternion.Euler(0, placeRotated ? 90 : 0, 0);

                string type = (ic.IcType == ICType.breadboard ? "0" : "1");

                if (fabricator.canPlaceBreadBoard(type, x, z, placeRotated))
                {
                    if (showsError)
                    {
                        showsError = false;
                        RemoveOutline(icModel.GetComponentsInChildren<Renderer>());
                    }
                }
                else
                {
                    if (!showsError)
                    {
                        showsError = true;
                        AddOutline(icModel.GetComponentsInChildren<Renderer>());
                    }
                }
            }

        }
    }

    private void PlaceActionWire_Hover()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit) && !EventSystem.current.IsPointerOverGameObject())
        {

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                icModel.transform.position = GameObjectPool;
                return;
            }

            bool isPin = hit.transform.gameObject.layer == LayerMask.NameToLayer("Pin");

            string nodeId = hit.transform.gameObject.name;
            int index = 0;                    
            int x = Mathf.RoundToInt(hit.point.x);
            int z = Mathf.RoundToInt(hit.point.z);

            if (isPin)
            {
                index = CircuitHelper.GetIndexFromNode(nodeId, hit.point);
            }

            Vector3 location = new Vector3(x, 0, z);


            if (locationASelected)
            {
                List<Vector3> hoverLinePos = new List<Vector3>(wireNodes);
                //hoverLinePos.Add(hit.point);
                hoverLinePos.Add(new Vector3(Mathf.Round(hit.point.x * 2) / 2, hit.point.y, Mathf.Round(hit.point.z * 2) / 2));
                icModel.GetComponentInChildren<LineRenderer>().positionCount = hoverLinePos.Count;
                icModel.GetComponentInChildren<LineRenderer>().SetPositions(hoverLinePos.ToArray());
            }
            else
            {
                icModel.transform.position = location;
            }

            if (isPin && circuitPool.CanPlace(ic.IcType, ic.Pins, nodeId, index))
            {
                if (showsError)
                {
                    showsError = false;
                    RemoveOutline(icModel.GetComponentsInChildren<Renderer>());
                }
            }
            else
            {
                if (!showsError)
                {
                    showsError = true;
                    AddOutline(icModel.GetComponentsInChildren<Renderer>());
                }
            }

        }
        else if (icModel != null)
        {
            if (ic.IcType != ICType.wire)
            {
                icModel.transform.position = GameObjectPool;
            }
        }
    }

    private void PlaceAction_Hover()
    {
        if(ic != null && ic.IcType == ICType.wire)
        {
            PlaceActionWire_Hover();
            return;
        }

        if (ic != null && (ic.IcType == ICType.breadboard || ic.IcType == ICType.powerrail))
        {
            PlaceActionBreadboard_Hover();
            return;
        }

        int layer_mask = LayerMask.GetMask("Pin");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 500, layer_mask) && !EventSystem.current.IsPointerOverGameObject())
        {
            string nodeId = hit.transform.gameObject.name;           
            int index = CircuitHelper.GetIndexFromNode(nodeId, hit.point);
            int x = Mathf.RoundToInt(hit.point.x);
            int z = Mathf.RoundToInt(hit.point.z);

            if (ic != null && icModel != null)
            {

                Vector3 location = new Vector3(x, 0, z);

                switch (ic.IcType)
                {
                    case ICType.dual:
                        break;
                    default:
                        icModel.transform.position = location;
                        icModel.transform.rotation = Quaternion.Euler(0, CircuitHelper.IsNodeRotated(nodeId) ? 90 : 0, 0);
                        break;
                }                

                if (circuitPool.CanPlace(ic.IcType, ic.Pins, nodeId, index))
                {
                    if (showsError)
                    {
                        showsError = false;
                        RemoveOutline(icModel.GetComponentsInChildren<Renderer>());
                    }
                }
                else
                {
                    if (!showsError)
                    {
                        showsError = true;
                        AddOutline(icModel.GetComponentsInChildren<Renderer>());
                    }
                }
            }

        }
        else if(icModel != null)
        {
            if (ic.IcType != ICType.wire)
            {
                icModel.transform.position = GameObjectPool;
                icModel.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    private void RemoveAction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Component"))
            {
                if (hit.transform.tag == "Wire")
                {
                    circuitPool.RemoveWire(hit.transform.parent.name);
                }
                else
                {
                    circuitPool.RemoveIntegratedCircuit(hit.transform.parent.name);
                }
                Destroy(hit.transform.parent.gameObject);
            }
        }
    }

    private void DemolishAction()
    {
        int layer_mask = LayerMask.GetMask("Breadboard");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 500, layer_mask) && !EventSystem.current.IsPointerOverGameObject())
        {
            HashSet<string> nodesToDelete = new HashSet<string>();
            HashSet<Guid> componentsToDelete = new HashSet<Guid>();
            HashSet<Guid> wiresToDelete = new HashSet<Guid>();

            for (int i = 0; i < hit.transform.childCount; i++)
            {
                string id = hit.transform.GetChild(i).name;
                if (id == "none")
                {
                    continue;
                }                

                foreach(string s in breadBoard.nodes.Keys)
                {
                    if (s.StartsWith(id))
                    {
                        nodesToDelete.Add(s);
                        foreach (Connection c in breadBoard.nodes[s].connections.Values)
                        {
                            if (!c.IsNode)
                            {
                                componentsToDelete.Add(Guid.Parse(c.ID));
                            }
                        }
                    }
                }

                foreach(Guid g in breadBoard.components.Keys)
                {
                    IntegratedCircuit ic = breadBoard.components[g];
                    if (ic.IcType == ICType.wire)
                    {                        
                        if (ic.GetPinNode(0).StartsWith(id) || ic.GetPinNode(1).StartsWith(id))
                        {
                            wiresToDelete.Add(g);
                        }
                    }
                    else if (!ic.WriteToNodes)
                    {
                        if (ic.GetPinNode(0).StartsWith(id))
                        {
                            componentsToDelete.Add(g);
                        }
                    }
                }                
            }

            for (int i = fabricator.breadBoardData.Count - 1; i >= 0; i--)
            {
                if (hit.transform.name == fabricator.breadBoardData[i].ToString())
                {
                    fabricator.breadBoardData.RemoveAt(i);
                }
            }

            foreach(Guid g in componentsToDelete)
            {
                Destroy(breadBoard.components[g].GetObjRef());
                circuitPool.RemoveIntegratedCircuit(g.ToString());                          
            }
            foreach(Guid g in wiresToDelete)
            {
                Destroy(breadBoard.components[g].GetObjRef());
                circuitPool.RemoveWire(g.ToString());                
            }
            foreach(string s in nodesToDelete)
            {
                breadBoard.nodes.Remove(s);
            }
            Destroy(hit.transform.gameObject);

        }
    }

    private void RemoveAction_Hover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        int layer_mask = LayerMask.GetMask("Component");
        if (Physics.Raycast(ray, out RaycastHit hit, 500, layer_mask) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (hoverObject != hit.transform.gameObject)
            {
                if (hoverObject != null)
                {
                    RemoveOutline(hoverObject.GetComponentsInChildren<Renderer>());
                }

                AddOutline(hit.transform.parent.gameObject.GetComponentsInChildren<Renderer>());
                hoverObject = hit.transform.parent.gameObject;
            }
        }
        else if (hoverObject != null)
        {
            RemoveOutline(hoverObject.GetComponentsInChildren<Renderer>());
            hoverObject = null;
        }
    }

    private void DemolishAction_Hover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        int layer_mask = LayerMask.GetMask("Breadboard");
        if (Physics.Raycast(ray, out RaycastHit hit, 500, layer_mask) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (hoverObject != hit.transform.gameObject)
            {
                if (hoverObject != null)
                {
                    RemoveOutline(hoverObject.GetComponentsInChildren<Renderer>());
                }

                AddOutline(hit.transform.gameObject.GetComponentsInChildren<Renderer>());
                hoverObject = hit.transform.gameObject;
            }
        }
        else if (hoverObject != null)
        {
            RemoveOutline(hoverObject.GetComponentsInChildren<Renderer>());
            hoverObject = null;
        }
    }

    private void ProbeAction_Hover()
    {
        int layer_mask = LayerMask.GetMask("Pin");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 500, layer_mask) && !EventSystem.current.IsPointerOverGameObject())
        {
            string nodeId = hit.transform.gameObject.name;
            int index = CircuitHelper.GetIndexFromNode(nodeId, hit.point);

            string stateString;

            int state = breadBoard.GetNodeState(hit.transform.name);
            if (state == 1)
            {
                stateString = "High";
            }
            else if (state == -1)
            {
                stateString = "Low";
            }
            else
            {
                stateString = "Off";
            }

            string[] s = nodeId.Split('x');
            bool rotated = CircuitHelper.IsNodeRotated(nodeId);

            string pin = "";
            switch(s[0])
            {
                case "0":
                    pin = (index + 1).ToString() + (char)(int.Parse(s[3]) + 64);
                    break;
                case "1":
                    pin = s[3] + (char)(index + 65);
                    break;
                case "2":
                    pin = s[3] + (char)(index + 70);
                    break;
            }
            
            ProbeText.text = "Probing Node\nBoard: " + s[1] + "x" + s[2] + "\nPin: " + pin + "\n\nState: " + stateString;

            HighLight.transform.position = new Vector3(Mathf.RoundToInt(hit.point.x), 0, Mathf.RoundToInt(hit.point.z));

        }
        else
        {
            ProbeText.text = "Probing Node\nUnknown\n\nState\nUnknown";
            HighLight.transform.position = new Vector3(0, 500, 0);
        }
    }

    private void ProbeAction()
    {
        int layer_mask = LayerMask.GetMask("Pin", "UI");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 500, layer_mask) && !EventSystem.current.IsPointerOverGameObject())
        {

            oscilloscope.NodeA = hit.transform.name;

        }
        else
        {
            oscilloscope.NodeA = "";
        }
    }

    private void NoneAction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (hit.transform.tag == "Switch")
            {
                mono_Switch.TriggerSwitch(hit.transform.parent.name, int.Parse(hit.transform.name), hit.transform.gameObject);
            }
        }
    }

    public void PlaceComponent(string nodeId, int index, Vector3 location)
    {
        switch(ic.IcType){
            case ICType.dual:
                break;
            case ICType.wire:
                if (locationASelected)
                {
                    breadBoard.LinkNodes(nodeIdA, indexA, nodeId, index);
                    
                    Vector3 finalPos = CircuitHelper.GetPositionFromNode(nodeId, index);
                    finalPos.y = 0.25f;
                    wireNodes.Add(finalPos);

                    for(int i = wireNodes.Count - 2; i >= 0; i--)
                    {
                        Vector3[] vectors = WireHelper.CurvedPoints(wireNodes[i], wireNodes[i + 1]);
                        foreach(Vector3 v in vectors.Reverse())
                        {
                            float newY = finalPos.y;
                            RaycastHit hit;
                            if(Physics.Raycast(new Vector3(v.x, 200, v.z), -Vector3.up, out hit, 250))
                            {
                                newY += hit.point.y;
                            }

                            newY = Mathf.Max(newY, v.y);

                            wireNodes.Insert(i + 1, new Vector3(v.x, newY, v.z));
                        }
                    }

                    GameObject placedWire = circuitPool.PlaceIntegratedCircuit(icName, nodeId, index, nodeIdA, indexA, false);

                    LineRenderer lineRenderer = placedWire.GetComponentInChildren<LineRenderer>();                    
                    lineRenderer.positionCount = wireNodes.Count;
                    lineRenderer.SetPositions(wireNodes.ToArray());

                    CircuitHelper.CreateColliderChain(placedWire, wireNodes.ToArray());

                    Wire wire = (Wire)breadBoard.GetIntegratedCircuit(Guid.Parse(placedWire.name));
                    wire.points = wireNodes.ToArray();

                    ResetWire();
                }
                else
                {
                    wireNodes.Clear();
                    locationASelected = true;
                    nodeIdA = nodeId;
                    indexA = index;

                    Vector3 startPos = CircuitHelper.GetPositionFromNode(nodeId, index);
                    startPos.y = 0.25f;
                    wireNodes.Add(startPos);
                    icModel.transform.position = new Vector3(0, 0, 0);
                }
                break;
            default:
                GameObject placed = circuitPool.PlaceIntegratedCircuit(icName, nodeId, index, false);
                placed.transform.position = location;
                if (CircuitHelper.IsNodeRotated(nodeId))
                {
                    placed.transform.rotation = Quaternion.Euler(0, 90, 0);
                }

                break;
        }
    }

    

    public void AttachIC(string name)
    {
        ClearIC();

        icName = name;
        ic = circuitPool.GetIntegratedCircuit(icName);
        icModel = circuitPool.GetIntegratedCircuitObj(ic);
        icModel = Instantiate(icModel, GameObjectPool, Quaternion.identity) as GameObject;
    }

    public void ClearIC()
    {
        if (icModel != null)
        {
            Destroy(icModel);
        }
        icName = "";
        icModel = null;
        ic = null;
        hoverObject = null;
        hoverMaterials.Clear();
        showsError = false;
        placeRotated = false;
    }

    public void ResetWire()
    {
        wireNodes.Clear();
        locationASelected = false;
        nodeIdA = "";
        indexA = 0;
        icModel.GetComponentInChildren<LineRenderer>().positionCount = 2;
        icModel.GetComponentInChildren<LineRenderer>().SetPositions(new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0) });
    }

    private void AddOutline(Renderer[] renderers)
    {
        hoverMaterials.Clear();
        foreach(Renderer r in renderers)
        {
            hoverMaterials.Add(r.materials);
            r.materials = Enumerable.Repeat(outlineMat, r.materials.Length).ToArray();
        }        
    }

    private void RemoveOutline(Renderer[] renderers)
    {
        for(int i = 0; i < renderers.Length; i++)
        {
            renderers[i].materials = hoverMaterials[i];
        }        
    }

}

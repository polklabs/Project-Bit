using UnityEngine.EventSystems;
using UnityEngine;
using IntegratedCircuits;
using System.Linq;
using Helper;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public enum WorkspaceAction { Place, Remove, Probe, None };

public class WorkspaceEditor : MonoBehaviour
{
    //Referances
    public BreadBoard breadBoard;
    public CircuitPool circuitPool;
    public Mono_Switch mono_Switch;
    public Oscilloscope oscilloscope;

    public Material outlineMat;

    public WorkspaceAction Action;

    private readonly Vector3 GameObjectPool = new Vector3(0, 500, 0);

    private GameObject hoverObject = null;
    private Material[] hoverMaterials = null;

    /* For Placing Objects */
    private GameObject icModel = null;
    private string icName = "";
    private IntegratedCircuit ic = null;
    private bool locationASelected = false;
    private string nodeIdA = "";
    private int indexA = 0;
    private List<Vector3> wireNodes = new List<Vector3>();
    private bool showsError = false;

    /* For Probing */
    public Text ProbeText;
    public GameObject HighLight;

    void Update()
    {
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
                    PlaceActionWireNode();
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

    public void SetActionProbe()
    {
        ClearIC();
        if (Action == WorkspaceAction.Probe)
        {
            Action = WorkspaceAction.None;
            return;
        }
        Action = WorkspaceAction.Probe;
    }

    public void SetActionNone()
    {
        ClearIC();
        Action = WorkspaceAction.None;
    }

    public void SetActionPlace()
    {
        ClearIC();
        if (Action == WorkspaceAction.Place)
        {
            Action = WorkspaceAction.None;
            return;
        }
        Action = WorkspaceAction.Place;
    }

    private void PlaceAction()
    {
        int layer_mask = LayerMask.GetMask("Pin");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 500, layer_mask) && !EventSystem.current.IsPointerOverGameObject())
        {
            string nodeId = hit.transform.gameObject.name;
            int x = Mathf.RoundToInt(hit.point.x);
            int z = Mathf.RoundToInt(hit.point.z);

            if (ic != null && icModel != null)
            {
                string[] s = nodeId.Split('x');
                int index = 0;

                if (s[0].Equals("1") || s[0].Equals("2"))
                {
                    index = z - int.Parse(s[2]) - 1;
                }
                else if (s[0].Equals("0"))
                {
                    index = x - int.Parse(s[1]) - 1;
                }

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

    private void PlaceActionWireNode()
    {
        if (ic.IcType == ICType.wire)
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
                string[] s = nodeId.Split('x');

                if (s[0].Equals("1") || s[0].Equals("2"))
                {
                    index = z - int.Parse(s[2]) - 1;
                }
                else if (s[0].Equals("0"))
                {
                    index = x - int.Parse(s[1]) - 1;
                }
            }

            Vector3 location = new Vector3(x, 0, z);


            if (locationASelected)
            {
                List<Vector3> hoverLinePos = new List<Vector3>(wireNodes);
                hoverLinePos.Add(hit.point);
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
                    RemoveOutline(icModel.GetComponentInChildren<Renderer>());
                }
            }
            else
            {
                if (!showsError)
                {
                    showsError = true;
                    AddOutline(icModel.GetComponentInChildren<Renderer>());
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

        int layer_mask = LayerMask.GetMask("Pin");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 500, layer_mask) && !EventSystem.current.IsPointerOverGameObject())
        {
            string nodeId = hit.transform.gameObject.name;
            int x = Mathf.RoundToInt(hit.point.x);
            int z = Mathf.RoundToInt(hit.point.z);

            if (ic != null && icModel != null)
            {
                string[] s = nodeId.Split('x');
                int index = 0;

                if (s[0].Equals("1") || s[0].Equals("2"))
                {
                    index = z - int.Parse(s[2]) - 1;
                }
                else if (s[0].Equals("0"))
                {
                    index = x - int.Parse(s[1]) - 1;
                }

                Vector3 location = new Vector3(x, 0, z);

                switch (ic.IcType)
                {
                    case ICType.dual:
                        break;
                    case ICType.wire:
                        if (locationASelected)
                        {
                            List<Vector3> hoverLinePos = new List<Vector3>(wireNodes);                            
                            hoverLinePos.Add(hit.point);
                            icModel.GetComponentInChildren<LineRenderer>().positionCount = hoverLinePos.Count;
                            icModel.GetComponentInChildren<LineRenderer>().SetPositions(hoverLinePos.ToArray());
                        }
                        else
                        {
                            icModel.transform.position = location;
                        }
                        break;
                    default:
                        icModel.transform.position = location;
                        break;
                }                

                if (circuitPool.CanPlace(ic.IcType, ic.Pins, nodeId, index))
                {
                    if (showsError)
                    {
                        showsError = false;
                        RemoveOutline(icModel.GetComponentInChildren<Renderer>());
                    }
                }
                else
                {
                    if (!showsError)
                    {
                        showsError = true;
                        AddOutline(icModel.GetComponentInChildren<Renderer>());
                    }
                }
            }

        }
        else if(icModel != null)
        {
            if (ic.IcType != ICType.wire)
            {
                icModel.transform.position = GameObjectPool;
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

    private void RemoveAction_Hover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Component"))
            {
                if (hoverObject != hit.transform.gameObject)
                {
                    if (hoverObject != null)
                    {
                        RemoveOutline(hoverObject.GetComponentInChildren<Renderer>());
                    }

                    AddOutline(hit.transform.parent.gameObject.GetComponentInChildren<Renderer>());
                    hoverObject = hit.transform.parent.gameObject;
                }
            }
            else if(hoverObject != null)
            {
                RemoveOutline(hoverObject.GetComponentInChildren<Renderer>());
                hoverObject = null;
            }
        }
        else if (hoverObject != null)
        {
            RemoveOutline(hoverObject.GetComponentInChildren<Renderer>());
            hoverObject = null;
        }
    }

    private void ProbeAction_Hover()
    {
        int layer_mask = LayerMask.GetMask("Pin");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 500, layer_mask) && !EventSystem.current.IsPointerOverGameObject())
        {
            string stateString;
            //Vector2Int state = breadBoard.GetNodeStateFull(hit.transform.name);
            //stateString =  state.x.ToString() + " : " + state.y.ToString();

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


            ProbeText.text = "Probing Node\n" + hit.transform.name + "\n\nState\n" + stateString;

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
                mono_Switch.TriggerSwitch(hit.transform.parent.name, int.Parse(hit.transform.name));
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

                Debug.Log("Placing at " + nodeId + index);
                GameObject placed = circuitPool.PlaceIntegratedCircuit(icName, nodeId, index, false);
                placed.transform.position = location;

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
        hoverMaterials = null;
        showsError = false;
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

    private void AddOutline(Renderer r)
    {
        hoverMaterials = r.materials;
        r.materials = Enumerable.Repeat(outlineMat, r.materials.Length).ToArray();   
    }

    private void RemoveOutline(Renderer r)
    {
        r.materials = hoverMaterials;
    }

}

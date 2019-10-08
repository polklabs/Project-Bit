using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;
using IntegratedCircuits;
using System.Linq;
using Helper;

public enum WorkspaceAction { Place, Remove, Probe, None };

public class WorkspaceEditor : MonoBehaviour
{
    //Referances
    public BreadBoard breadBoard;
    public CircuitPool circuitPool;
    public Mono_Switch mono_Switch;

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
    private bool showsError = false;

    void Update()
    {
        switch (Action)
        {
            case WorkspaceAction.Place:
                PlaceAction_Hover();
                break;
            case WorkspaceAction.Probe:
                break;
            case WorkspaceAction.Remove:
                RemoveAction_Hover();
                break;
            default:
                break;
        }

        if (Input.GetMouseButtonDown(0) )//&& !EventSystem.current.IsPointerOverGameObject())
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
        int layer_mask = LayerMask.GetMask("Pin", "UI");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (/*!EventSystem.current.IsPointerOverGameObject() && */Physics.Raycast(ray, out RaycastHit hit, 500, layer_mask))
        {

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("UI"))
            {                
                return;
            }

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
        else if (icModel != null)
        {
            icModel.transform.position = GameObjectPool;
        }
    }

    private void PlaceAction_Hover()
    {
        int layer_mask = LayerMask.GetMask("Pin", "UI");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (/*!EventSystem.current.IsPointerOverGameObject() && */Physics.Raycast(ray, out RaycastHit hit, 500, layer_mask))
        {

            if(hit.transform.gameObject.layer == LayerMask.NameToLayer("UI") && icModel != null)
            {
                icModel.transform.position = GameObjectPool;
                return;
            }

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
                            Vector3 locationA = CircuitHelper.GetPositionFromNode(nodeIdA, indexA);

                            icModel.transform.position = new Vector3((locationA.x + x) / 2, 0, (locationA.z + z) / 2);
                            icModel.transform.localScale = new Vector3(Vector3.Distance(location, locationA) / 2, 1, 1);
                            //icModel.transform.Rotate(0, CircuitHelper.AngleBetweenVector3(location, locationA), 0);
                            float angle = CircuitHelper.AngleBetweenVector3(location, locationA);
                            angle = locationA.z > location.z ? -angle : angle;
                            icModel.transform.rotation = Quaternion.Euler(0, angle, 0);
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
            icModel.transform.position = GameObjectPool;
        }
    }

    private void RemoveAction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
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

        if (Physics.Raycast(ray, out RaycastHit hit))
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
    }

    private void ProbeAction()
    {

    }

    private void NoneAction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
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

                    Vector3 locationA = CircuitHelper.GetPositionFromNode(nodeIdA, indexA);

                    GameObject placedWire = circuitPool.PlaceIntegratedCircuit(icName, nodeId, index, nodeIdA, indexA, false);
                    placedWire.transform.position = new Vector3((locationA.x + location.x) / 2, 0, (locationA.z + location.z) / 2);
                    placedWire.transform.localScale = new Vector3(Vector3.Distance(location, locationA) / 2, 1, 1);
                    float angle = CircuitHelper.AngleBetweenVector3(location, locationA);
                    angle = locationA.z > location.z ? -angle : angle;
                    placedWire.transform.rotation = Quaternion.Euler(0, angle, 0);

                    locationASelected = false;
                    nodeIdA = "";
                    indexA = 0;
                }
                else
                {
                    locationASelected = true;
                    nodeIdA = nodeId;
                    indexA = index;
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
        //icModel.transform.parent = 

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

    private void AddOutline(Renderer r)
    {
        hoverMaterials = r.materials;
        r.materials = Enumerable.Repeat(outlineMat, r.materials.Length).ToArray();
        ////Renderer rNew = obj.GetComponent<Renderer>();
        //List<Material> materialsNew = new List<Material>(r.materials);
        //materialsNew.Add(outlineMat);
        //r.materials = materialsNew.ToArray();        
    }

    private void RemoveOutline(Renderer r)
    {
        r.materials = hoverMaterials;
        ////Renderer r = obj.GetComponent<Renderer>();
        //List<Material> materialsOld = new List<Material>(r.materials);
        //materialsOld.RemoveAt(materialsOld.Count - 1);
        //r.materials = materialsOld.ToArray();
    }

}

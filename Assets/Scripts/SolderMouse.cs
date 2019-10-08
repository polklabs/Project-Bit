using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Helper;

public enum Action { Place, Remove, Probe, None };

public class SolderMouse : MonoBehaviour
{    
    public CircuitPool circuitPool;
    public Mono_Switch mono_Switch;
    public Transform pool;

    public GameObject highlight;
    public BreadBoard breadBoard;

    public bool isMultipoint;
    public GameObject integratedCircuitModel;
    public string integratedCircuitName;
    public IntegratedCircuits.IntegratedCircuit integratedCircuit;

    /* Used For multipoint circuits */
    private Vector3 locationA;
    private bool locationASelected = false;
    private string nodeIdA;
    private int indexA;


    public Action action = Action.None;

    public void SetActionRemove()
    {
        ClearIC();
        if (action == Action.Remove)
        {
            action = Action.None;
            return;
        }
        action = Action.Remove;        
    }

    public void SetActionProbe()
    {
        ClearIC();
        if (action == Action.Probe)
        {
            action = Action.None;
            return;
        }
        action = Action.Probe;
    }

    public void SetActionNone()
    {
        ClearIC();
        action = Action.None;
    }

    public void SetActionPlace()
    {
        ClearIC();
        if (action == Action.Place)
        {
            action = Action.None;
            return;
        }
        action = Action.Place;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {            
            if (action == Action.Place || action == Action.Probe)
            {
                int layer_mask = LayerMask.GetMask("Pin", "UI");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 500, layer_mask))
                {

                    string nodeId = hit.transform.gameObject.name;
                    int x = Mathf.RoundToInt(hit.point.x);
                    int z = Mathf.RoundToInt(hit.point.z);
                    string[] s = nodeId.Split('x');
                    int index = 0;

                    if (s[0].Equals("1") || s[0].Equals("2"))
                    {
                        index = (z - int.Parse(s[2])) - 1;
                    }
                    else if (s[0].Equals("0"))
                    {
                        index = (x - int.Parse(s[1])) - 1;
                    }

                    if (action == Action.Place)
                    {
                        if (integratedCircuitModel != null)
                        {
                            if (isMultipoint)
                            {
                                PlaceMultipointCirciut(nodeId, index, x, z);
                            }
                            else
                            {
                                PlaceSimpleCircuit(nodeId, index, x, z);
                                //ClearIC();
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("State: " + breadBoard.GetNodeState(nodeId).ToString());
                    }
                }                
            }
            else if(action == Action.Remove)
            {
                int layer_mask = LayerMask.GetMask("Component", "UI");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 500, layer_mask))
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
            else
            {
                int layer_mask = LayerMask.GetMask("Switch", "UI");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 500, layer_mask))
                {
                    mono_Switch.TriggerSwitch(hit.transform.parent.name, int.Parse(hit.transform.name));
                }
            }

            if (action != Action.Probe && !isMultipoint)
            {
                //SetActionNone();
            }
        }

        UpdateHighlight();

    }



    public void PlaceMultipointCirciut(string nodeId, int index, int x, int z)
    {
        if (!locationASelected)
        {
            locationA = new Vector3(x, 0, z);
            locationASelected = true;
            nodeIdA = nodeId;
            indexA = index;
        }
        else
        {
            breadBoard.LinkNodes(nodeIdA, indexA, nodeId, index);

            Vector3 locationB = new Vector3(x, 0, z);
            locationASelected = false;

            GameObject placed = circuitPool.PlaceIntegratedCircuit(integratedCircuitName, nodeId, index, nodeIdA, indexA, false);
            placed.transform.position = new Vector3((locationA.x + locationB.x) / 2, 0, (locationA.z + locationB.z) / 2);            
            placed.transform.localScale = new Vector3(Vector3.Distance(locationB, locationA) / 2, 1, 1);
            placed.transform.Rotate(0, CircuitHelper.AngleBetweenVector3(locationB, locationA), 0);            
        }
    }

    public void PlaceSimpleCircuit(string nodeId, int index, int x, int z)
    {
        if (circuitPool.CanPlace(integratedCircuit.IcType, integratedCircuit.Pins, nodeId, index))
        {
            GameObject placed = circuitPool.PlaceIntegratedCircuit(integratedCircuitName, nodeId, index, false);
            placed.transform.position = new Vector3(x, 0, z);
        }
        else
        {
            Debug.Log("Cannot place here");
        }
    }    

    private void UpdateHighlight()
    {
        bool canPlace = false;
        Vector3 newPos;
        Vector3 poolPos = new Vector3(0, 500, 0);
        int layer_mask = LayerMask.GetMask("Pin");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out RaycastHit hit, 500, layer_mask))
        {
            string nodeId = hit.transform.gameObject.name;
            int x = Mathf.RoundToInt(hit.point.x);
            int z = Mathf.RoundToInt(hit.point.z);

            if (integratedCircuit != null)
            {
                string[] s = nodeId.Split('x');
                int index = 0;

                if (s[0].Equals("1") || s[0].Equals("2"))
                {
                    index = z - int.Parse(s[2]);
                }
                else if (s[0].Equals("0"))
                {
                    index = x - int.Parse(s[1]);
                }

                canPlace = circuitPool.CanPlace(integratedCircuit.IcType, integratedCircuit.Pins, nodeId, index - 1);
            }
            newPos = new Vector3(x, 0, z);       
        } else
        {
            newPos = poolPos;
        }

        highlight.transform.position = newPos;

        if (integratedCircuitModel != null)
        {
            if (canPlace)
            {
                integratedCircuitModel.transform.position = newPos;
            }
            else
            {
                integratedCircuitModel.transform.position = poolPos;
            }
        }       
    }
    
    public void AttachIC(string name)
    {
        ClearIC();

        integratedCircuitName = name;
        integratedCircuit = circuitPool.GetIntegratedCircuit(name);
        integratedCircuitModel = circuitPool.GetIntegratedCircuitObj(integratedCircuit);
        integratedCircuitModel = Instantiate(integratedCircuitModel, new Vector3(0, 500, 0), Quaternion.identity);
        integratedCircuitModel.transform.parent = pool;
        isMultipoint = false;
    }

    public void SetMultipoint()
    {
        isMultipoint = true;
    }

    public void ClearIC()
    {
        if (integratedCircuitModel != null)
        {
            Destroy(integratedCircuitModel);
        }
        integratedCircuitName = "";
        integratedCircuitModel = null;
        integratedCircuit = null;
        isMultipoint = false;
    }
}

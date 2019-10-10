using System;
using System.Collections;
using System.Collections.Generic;
using IntegratedCircuits;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Remoting;

public class CircuitPool : MonoBehaviour
{
    public BreadBoard breadBoard;
    public Transform parent;

    public GameObject PlaceIntegratedCircuit(string IcName, string nodeIdA, int indexA, bool skipUpdate)
    {
        IntegratedCircuit ic = GetIntegratedCircuit(IcName);
        return PlaceIntegratedCircuit(ic, nodeIdA, indexA, "", 0, skipUpdate);
    }
    public GameObject PlaceIntegratedCircuit(IntegratedCircuit ic, string nodeIdA, int indexA, bool skipUpdate)
    {
        return PlaceIntegratedCircuit(ic, nodeIdA, indexA, "", 0, skipUpdate);
    }
    public GameObject PlaceIntegratedCircuit(string IcName, string nodeIdA, int indexA, string nodeIdB, int indexB, bool skipUpdate)
    {
        IntegratedCircuit ic = GetIntegratedCircuit(IcName);
        return PlaceIntegratedCircuit(ic, nodeIdA, indexA, nodeIdB, indexB, skipUpdate);
    }
    public GameObject PlaceIntegratedCircuit(IntegratedCircuit ic, string nodeIdA, int indexA, string nodeIdB, int indexB, bool skipUpdate)
    {
        // Debug.Log("Placing: " + IcName);
        GameObject obj = Instantiate(GetIntegratedCircuitObj(ic), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        obj.transform.parent = parent;

        if (ic.NeedsObjRef)
        {
            ic.AssignObjRef(obj);
        }

        obj.name = ic.SetBreadBoard(breadBoard).ToString();

        switch (ic.IcType)
        {
            case ICType.solo:                
                breadBoard.SetNodeConnection(false, nodeIdA, indexA, 0, ic.GetId().ToString());
                ic.SetPinIndex(0, indexA);
                ic.SetPinNode(0, nodeIdA);
                break;
            case ICType.dual:

                breadBoard.SetNodeConnection(false, nodeIdA, indexA, 0, ic.GetId().ToString());
                ic.SetPinIndex(0, indexA);
                ic.SetPinNode(0, nodeIdA);

                breadBoard.SetNodeConnection(false, nodeIdB, indexB, 1, ic.GetId().ToString());
                ic.SetPinIndex(1, indexB);
                ic.SetPinNode(1, nodeIdB);

                break;
            case ICType.wire:
                ic.SetPinNode(0, nodeIdA);
                ic.SetPinIndex(0, indexA);
                ic.SetPinNode(1, nodeIdB);
                ic.SetPinIndex(1, indexB);
                break;
            case ICType.ic4:

                string[] id = nodeIdA.Split('x');
                int a = int.Parse(id[0]);
                int b = int.Parse(id[1]);
                int c = int.Parse(id[2]);
                int d = int.Parse(id[3]);

                for(int i = 0; i < 2; i++)
                {
                    for(int j = 0; j < ic.Pins/2; j++)
                    {
                        string nodeId = (a + i).ToString() + "x" + b.ToString() + "x" + (c + (i * 7)).ToString() + "x" + (d + j).ToString();
                        int index = i == 0 ? 4 : 0;
                        int pinIndex = i == 0 ? j : (ic.Pins - 1) - j;

                        breadBoard.SetNodeConnection(false, nodeId, index, pinIndex, ic.GetId().ToString());
                        ic.SetPinIndex(pinIndex, index);
                        ic.SetPinNode(pinIndex, nodeId);
                    }
                }               

                break;
            case ICType.ic6:

                id = nodeIdA.Split('x');
                a = int.Parse(id[0]);
                b = int.Parse(id[1]);
                c = int.Parse(id[2]);
                d = int.Parse(id[3]);

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < ic.Pins / 2; j++)
                    {
                        string nodeId = (a + i).ToString() + "x" + b.ToString() + "x" + (c + (i * 7)).ToString() + "x" + (d + j).ToString();
                        int index = i == 0 ? indexA : indexA - 1;
                        int pinIndex = i == 0 ? j : (ic.Pins - 1) - j;

                        breadBoard.SetNodeConnection(false, nodeId, index, pinIndex, ic.GetId().ToString());
                        ic.SetPinIndex(pinIndex, index);
                        ic.SetPinNode(pinIndex, nodeId);
                    }
                }

                break;
            default:
                return null;
        }

        breadBoard.components.Add(ic.GetId(), ic);

        if (!skipUpdate)
        {
            ic.Update();
        }

        return obj;
    }

    public void RemoveIntegratedCircuit(string id)
    {
        Guid guid = Guid.Parse(id);
        IntegratedCircuit ic = breadBoard.GetIntegratedCircuit(guid);
        ic.Update(true);
        for(int i = 0; i < ic.Pins; i++)
        {
            breadBoard.RemoveNodeConnection(ic.GetPinNode(i), ic.GetPinNodeIndex(i));
        }
        breadBoard.components.Remove(guid);
    }

    public void RemoveWire(string id)
    {
        Guid guid = Guid.Parse(id);
        IntegratedCircuit ic = breadBoard.GetIntegratedCircuit(guid);
        breadBoard.UnlinkNodes(ic.GetPinNode(0), ic.GetPinNodeIndex(0), ic.GetPinNode(1), ic.GetPinNodeIndex(1));
        breadBoard.components.Remove(guid);
    }

    public IntegratedCircuit GetIntegratedCircuit(string IcName)
    {
        return (IntegratedCircuit)Activator.CreateInstance(null, "IntegratedCircuits." + IcName).Unwrap();               
    }

    public GameObject GetIntegratedCircuitObj(IntegratedCircuit circuit)
    {
        GameObject obj = GetIntegratedCircuitObj(circuit.ModelName);

        if (circuit != null)
        {
            if (circuit.NeedsObjRef)
            {
                circuit.AssignObjRef(obj);
            }

            obj.name = circuit.SetBreadBoard(breadBoard).ToString();

            Text text = obj.GetComponentInChildren<Text>();
            if (text != null && text.text.Equals(""))
            {
                text.text = circuit.GetType().Name.Split('_')[0] + "\n\n" + circuit.GetId().ToString();
            }

        }

        return obj;

    }

    public GameObject GetIntegratedCircuitObj(string ModelName)
    {
        // Debug.Log(ModelName);
        return Resources.Load<GameObject>("Prefabs/" + ModelName);
    }

    public bool CanPlace(ICType iCType, int pins, string nodeId, int index)
    {
        switch (iCType)
        {
            case ICType.solo:
                return CanPlaceSinglePin(nodeId, index);
            case ICType.dual:
                return CanPlaceSinglePin(nodeId, index);
            case ICType.wire:
                return CanPlaceSinglePin(nodeId, index);
            case ICType.ic4:

                if (index != 4)
                {
                    return false;
                }

                string[] id = nodeId.Split('x');
                int a = int.Parse(id[0]);
                int b = int.Parse(id[1]);
                int c = int.Parse(id[2]);
                int d = int.Parse(id[3]);

                if(a != 1)
                {
                    return false;
                }

                if(d > 64 - (pins / 2))
                {
                    return false;
                }

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < pins / 2; j++)
                    {
                        string nodeIdT = (a + i).ToString() + "x" + b.ToString() + "x" + (c + (i * 7)).ToString() + "x" + (d + j).ToString();
                        int indexT = i == 0 ? 4 : 0;
                        
                        if(!CanPlaceSinglePin(nodeIdT, indexT))
                        {
                            return false;
                        }
                        
                    }
                }
                return true;                

            case ICType.ic6:

                if (index == 0)
                {
                    return false;
                }

                id = nodeId.Split('x');
                a = int.Parse(id[0]);
                // b = int.Parse(id[1]);
                c = int.Parse(id[2]);
                d = int.Parse(id[3]);

                if (a != 1)
                {
                    return false;
                }

                if (d > 64 - (pins / 2))
                {
                    return false;
                }

                //for (int i = 0; i < 2; i++)
                //{
                //    for (int j = 0; j < pins / 2; j++)
                //    {
                //        string nodeIdT = (a + i).ToString() + "x" + b.ToString() + "x" + (c + (i * 7)).ToString() + "x" + (d + j).ToString();
                //        int indexT = i == 0 ? index : index - 1;

                //        if (!CanPlaceSinglePin(nodeIdT, indexT))
                //        {
                //            return false;
                //        }

                //    }
                //}

                for(int i = index; i < 5; i++)
                {
                    for(int k = 0; k < pins / 2; k++)
                    {
                        string nodeIdT = "1x" + id[1] + "x" + id[2] + "x" + (d + k).ToString();
                        if (!CanPlaceSinglePin(nodeIdT, i))
                        {
                            return false;
                        }
                    }
                }

                for (int i = 0; i < index; i++)
                {
                    for (int k = 0; k < pins / 2; k++)
                    {
                        string nodeIdT = "2x" + id[1] + "x" + (c + 7).ToString() + "x" + (d + k).ToString();
                        if (!CanPlaceSinglePin(nodeIdT, i))
                        {
                            return false;
                        }
                    }
                }

                return true;

            default:
                return false;
        }
    }

    private bool CanPlaceSinglePin(string nodeId, int index)
    {
        return !breadBoard.NodeHasConnection(nodeId, index);        
    }

}

using UnityEngine;
using System.Collections.Generic;
using System;
using IntegratedCircuits;
using System.Linq;

/// <summary>
/// Handles the placing of different types of breadboards
/// </summary>
public class Fabricator : Singleton<Fabricator>
{
    public List<BreadBoardData> breadBoardData = new List<BreadBoardData>();

    public Transform breadBoardParent;

    public GameObject pinZ;
    public GameObject pinX;
    public GameObject breadBoardPrefab;
    public GameObject powerRailPrefab;

    private BreadBoard breadBoard;
    private CircuitPool circuitPool;

    new void Awake()
    {
        breadBoard = BreadBoard.GetInstance();
        circuitPool = CircuitPool.GetInstance();
    }

    public void loadFromBreadBoardData()
    {
        foreach (BreadBoardData bbd in breadBoardData)
        {
            bbd.objRef = createBoard(bbd.t, bbd.x, bbd.y, bbd.r);
        }
    }

    public void RemoveWorkspace()
    {
        foreach (var bb in breadBoardData.ToArray())
        {
            RemoveBreadBoard(bb.objRef);
        }
    }

    public void RemoveBreadBoard(GameObject boardObj)
    {
        HashSet<string> nodesToDelete = new HashSet<string>();
        HashSet<Guid> componentsToDelete = new HashSet<Guid>();
        HashSet<Guid> wiresToDelete = new HashSet<Guid>();

        for (int i = 0; i < boardObj.transform.childCount; i++)
        {
            string id = boardObj.transform.GetChild(i).name;
            if (id == "none")
            {
                continue;
            }

            foreach (string s in breadBoard.nodes.Keys.Where(x => x.StartsWith(id)))
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

            foreach (Guid g in breadBoard.components.Keys)
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

        for (int i = breadBoardData.Count - 1; i >= 0; i--)
        {
            if (boardObj.transform.name == breadBoardData[i].ToString())
            {
                breadBoardData.RemoveAt(i);
            }
        }

        foreach (Guid g in componentsToDelete)
        {
            Destroy(breadBoard.components[g].GetObjRef());
            circuitPool.RemoveIntegratedCircuit(g.ToString());
        }
        foreach (Guid g in wiresToDelete)
        {
            Destroy(breadBoard.components[g].GetObjRef());
            circuitPool.RemoveWire(g.ToString());
        }
        foreach (string s in nodesToDelete)
        {
            breadBoard.nodes.Remove(s);
        }
        Destroy(boardObj.transform.gameObject);
    }

    public void addBoard(string t, int x, int y, bool rotation)
    {
        var bbd = new BreadBoardData(t, x, y, rotation);
        breadBoardData.Add(bbd);
        bbd.objRef = createBoard(t, x, y, rotation);
    }

    public bool canPlaceBreadBoard(string t, int x, int z, bool rotation)
    {
        foreach (BreadBoardData bb in breadBoardData)
        {
            if(Helper.CircuitHelper.breadboardsOverlap(t, x, z, rotation, bb.t, bb.x, bb.y, bb.r))
            {
                return false;
            }
        }
        return true;
    }

    GameObject createBoard(string t, int x, int y, bool rotation)
    {
        GameObject obj;
        if (t == "0")
        {
            if (rotation)
            {
                obj = placeBreadBoard(3, x, y, rotation, null);
                placeBreadBoard(0, x, y, rotation, obj);
                placeBreadBoard(0, x + 18, y, rotation, obj);
                placeBreadBoard(1, x + 4, y, rotation, obj);
            }
            else
            {
                obj = placeBreadBoard(3, x, y, rotation, null);
                placeBreadBoard(0, x, y, rotation, obj);
                placeBreadBoard(0, x, y + 18, rotation, obj);
                placeBreadBoard(1, x, y + 4, rotation, obj);
            }
        } else
        {
            obj = placeBreadBoard(4, x, y, rotation, null);
            placeBreadBoard(0, x, y, rotation, obj);
        }
        return obj;
    }

    GameObject placeBreadBoard(int type, int x, int y, bool rotation, GameObject parent)
    {
        // Power rail
        if(type == 0)
        {
            placePowerRail(type, x, y, rotation, parent);
        }
        else if (type == 3)
        {
            GameObject obj = Instantiate(breadBoardPrefab, new Vector3(x + (rotation ? 10.5f : 32), 0, y + (rotation ? -32 : 10.5f)), Quaternion.identity) as GameObject;
            obj.transform.parent = breadBoardParent;
            obj.name = string.Format("0x{1}x{2}x{3}", type, x, y, rotation ? "1" : "0");

            if (rotation)
            {
                obj.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            return obj;
        }
        else if (type == 4)
        {
            GameObject obj = Instantiate(powerRailPrefab, new Vector3(x + (rotation ? 1.5f : 32), 0, y + (rotation ? -32 : 1.5f)), Quaternion.identity) as GameObject;
            obj.transform.parent = breadBoardParent;
            obj.name = string.Format("1x{1}x{2}x{3}", type, x, y, rotation ? "1" : "0");

            if (rotation)
            {
                obj.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            return obj;
        }
        else
        {
            placeMainBoard(type, x, y, rotation, parent);
            if (rotation)
            {
                placeMainBoard(type + 1, x + 7, y, rotation, parent);
            }
            else
            {
                placeMainBoard(type + 1, x, y + 7, rotation, parent);
            }
        }
        return null;
    }

    void placePowerRail(int type, int x, int y, bool rotation, GameObject obj)
    {
        GameObject parent = new GameObject();        
        parent.transform.position = new Vector3(x, 0, y);
        parent.name = string.Format("{0}x{1}x{2}", type, x, y);

        int index = 1;
        for (int i = 1; i < 64; i++)
        {            
            if (index % 6 != 0)
            {
                if (index % 3 == 0)
                {
                    for (int j = 1; j < 3; j++)
                    {
                        GameObject temp = Instantiate(pinX, new Vector3(i + x, -0.4f, j + y), Quaternion.Euler(0, 0, 0)) as GameObject;
                        temp.name = string.Format("{0}x{1}x{2}", parent.name, j, (rotation ? "1" : "0"));
                        temp.transform.parent = parent.transform;
                        breadBoard.AddNode(temp.name, type, temp.GetComponent<MeshRenderer>());
                    }
                }
            }
            
            if (i == 21 || i == 36)
            {
                i++;
            }
            if (i == 28)
            {
                i += 2;
            }
            index++;
        }

        if (rotation)
        {
            parent.transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        parent.transform.parent = obj.transform;
    }

    void placeMainBoard(int type, int x, int y, bool rotation, GameObject obj)
    {
        GameObject parent = new GameObject();        
        parent.transform.position = new Vector3(x, 0, y);
        parent.name = string.Format("{0}x{1}x{2}", type, x, y);

        for (int i = 1; i < 64; i++)
        {
            GameObject temp = Instantiate(pinZ, new Vector3(i + x, -0.4f, 3 + y), Quaternion.Euler(0, 0, 0)) as GameObject;
            temp.name = string.Format("{0}x{1}x{2}", parent.name, i, (rotation ? "1" : "0"));
            temp.transform.parent = parent.transform;
            breadBoard.AddNode(temp.name, type, temp.GetComponent<MeshRenderer>());       
        }

        if (rotation)
        {
            parent.transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        parent.transform.parent = obj.transform;
    }

}

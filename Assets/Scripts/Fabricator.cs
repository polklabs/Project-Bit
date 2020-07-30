using UnityEngine;
using System.Collections.Generic;

public class BreadBoardData
{
    public BreadBoardData(string _t, int _x, int _y, bool _r)
    {
        t = _t;
        x = _x;
        y = _y;
        r = _r;
    }

    public string t;
    public int x;
    public int y;
    public bool r;

    public override string ToString()
    {
        return string.Format("{0}x{1}x{2}x{3}", t, x.ToString(), y.ToString(), r ? "1" : "0");
    }
}

public class Fabricator : MonoBehaviour
{
    public List<BreadBoardData> breadBoardData = new List<BreadBoardData>();

    public Transform breadBoardParent;

    public GameObject pinZ;
    public GameObject pinX;
    public GameObject breadBoardPrefab;
    public GameObject powerRailPrefab;

    public BreadBoard breadBoard;

    public void loadFromBreadBoardData()
    {
        foreach (BreadBoardData bbd in breadBoardData)
        {
            createBoard(bbd.t, bbd.x, bbd.y, bbd.r);
        }
    }

    public void addBoard(string t, int x, int y, bool rotation)
    {
        breadBoardData.Add(new BreadBoardData(t, x, y, rotation));
        createBoard(t, x, y, rotation);
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

    void createBoard(string t, int x, int y, bool rotation)
    {
        if (t == "0")
        {
            if (rotation)
            {
                GameObject obj = placeBreadBoard(3, x, y, rotation, null);
                placeBreadBoard(0, x, y, rotation, obj);
                placeBreadBoard(0, x + 18, y, rotation, obj);
                placeBreadBoard(1, x + 4, y, rotation, obj);
            }
            else
            {
                GameObject obj = placeBreadBoard(3, x, y, rotation, null);
                placeBreadBoard(0, x, y, rotation, obj);
                placeBreadBoard(0, x, y + 18, rotation, obj);
                placeBreadBoard(1, x, y + 4, rotation, obj);
            }
        } else
        {
            GameObject obj = placeBreadBoard(4, x, y, rotation, null);
            placeBreadBoard(0, x, y, rotation, obj);
        }
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
                        breadBoard.AddNode(temp.name, type);
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
            breadBoard.AddNode(temp.name, type);       
        }

        if (rotation)
        {
            parent.transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        parent.transform.parent = obj.transform;
    }

}

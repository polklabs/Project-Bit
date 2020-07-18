using UnityEngine;

public class Fabricator : MonoBehaviour
{
    public Transform breadBoardParent;

    public GameObject pinZ;
    public GameObject pinX;
    public GameObject breadBoardPrefab;

    public BreadBoard breadBoard;

    // Start is called before the first frame update
    void Start()
    {
        newBoard(0, 0, false);
        newBoard(0, -1, true);

        newBoard(0, 25, false);

        newBoard(-25, 66, true);
    }

    void newBoard(int x, int y, bool rotation)
    {
        if (rotation)
        {
            placeBreadBoard(0, x, y, rotation);
            placeBreadBoard(0, x + 18, y, rotation);
            placeBreadBoard(1, x + 4, y, rotation);
            placeBreadBoard(3, x, y, rotation);
        }
        else
        {
            placeBreadBoard(0, x, y, rotation);
            placeBreadBoard(0, x, y + 18, rotation);
            placeBreadBoard(1, x, y + 4, rotation);
            placeBreadBoard(3, x, y, rotation);
        }
    }

    void placeBreadBoard(int type, int x, int y, bool rotation)
    {
        // Power rail
        if(type == 0)
        {
            placePowerRail(type, x, y, rotation);
        }
        else if (type == 3)
        {
            GameObject obj = Instantiate(breadBoardPrefab, new Vector3(x + (rotation ? 10.5f : 32), 0, y + (rotation ? -32 : 10.5f)), Quaternion.identity) as GameObject;
            obj.transform.parent = breadBoardParent;

            if (rotation)
            {
                obj.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
        }
        else
        {
            placeMainBoard(type, x, y, rotation);
            if (rotation)
            {
                placeMainBoard(type + 1, x + 7, y, rotation);
            }
            else
            {
                placeMainBoard(type + 1, x, y + 7, rotation);
            }
        }
    }

    void placePowerRail(int type, int x, int y, bool rotation)
    {
        GameObject parent = new GameObject();
        parent.transform.parent = breadBoardParent;
        parent.transform.position = new Vector3(x, 0, y);
        parent.name = type.ToString() + "x" + x.ToString() + "x" + y.ToString();

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
                        temp.name = parent.name + "x" + j.ToString() + "x" + (rotation ? "1" : "0");
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
    }

    void placeMainBoard(int type, int x, int y, bool rotation)
    {
        GameObject parent = new GameObject();
        parent.transform.parent = breadBoardParent;
        parent.transform.position = new Vector3(x, 0, y);
        parent.name = type.ToString() + "x" + x.ToString() + "x" + y.ToString();

        for (int i = 1; i < 64; i++)
        {
            GameObject temp = Instantiate(pinZ, new Vector3(i + x, -0.4f, 3 + y), Quaternion.Euler(0, 0, 0)) as GameObject;
            temp.name = parent.name + "x" + i.ToString() + "x" + (rotation ? "1" : "0");
            temp.transform.parent = parent.transform;
            breadBoard.AddNode(temp.name, type);       
        }

        if (rotation)
        {
            parent.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }

}

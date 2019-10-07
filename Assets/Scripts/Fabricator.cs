using System.Collections;
using System.Collections.Generic;
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
        placeBreadBoard(0, 0, 0, false);        
        placeBreadBoard(0, 0, 18, false);
        placeBreadBoard(1, 0, 4, false);
        placeBreadBoard(3, 0, 0, false);
    }

    // Update is called once per frame
    void Update()
    {
        
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
            GameObject obj = Instantiate(breadBoardPrefab, new Vector3(x + 32, 0, y + 10.5f), Quaternion.identity) as GameObject;
            obj.transform.parent = breadBoardParent;
        }
        else
        {
            placeMainBoard(type, x, y, rotation);
            placeMainBoard(type + 1, x, y + 7, rotation);
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
                        GameObject temp = Instantiate(pinX, new Vector3(i + x, -0.4f, j + y), Quaternion.identity) as GameObject;
                        temp.name = parent.name + "x" + j.ToString();
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
    }

    void placeMainBoard(int type, int x, int y, bool rotation)
    {
        GameObject parent = new GameObject();
        parent.transform.parent = breadBoardParent;
        parent.transform.position = new Vector3(x, 0, y);
        parent.name = type.ToString() + "x" + x.ToString() + "x" + y.ToString();

        for (int i = 1; i < 64; i++)
        {
            GameObject temp = Instantiate(pinZ, new Vector3(i + x, -0.4f, 3 + y), Quaternion.identity) as GameObject;
            temp.name = parent.name + "x" + i.ToString();
            temp.transform.parent = parent.transform;
            breadBoard.AddNode(temp.name, type);       
        }
    }

}

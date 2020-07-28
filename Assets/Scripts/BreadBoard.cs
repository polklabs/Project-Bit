using System;
using System.Collections.Generic;
using UnityEngine;
using IntegratedCircuits;

public class BreadBoard : MonoBehaviour
{
    public GeneralManager generalManager;

    public readonly Dictionary<Guid, IntegratedCircuit> components = new Dictionary<Guid, IntegratedCircuit>();
    public Dictionary<string, Node> nodes = new Dictionary<string, Node>();
    public Queue<Guid> updates = new Queue<Guid>();

    public void UpdateComponent(Guid id)
    {
        if (components.ContainsKey(id))
        {            
            updates.Enqueue(id);
        }
    }

    public void Update()
    {
        if (!generalManager.paused)
        {
            while (updates.Count > 0)
            {
                Guid id = updates.Dequeue();
                if (components.ContainsKey(id))
                {
                    components[id].Update();
                }

                if (generalManager.paused)
                {
                    break;
                }
            }
        } else
        {
            if (generalManager.step > 0 && updates.Count > 0)
            {
                Guid id = updates.Dequeue();
                if (components.ContainsKey(id))
                {
                    components[id].Update();
                }
                generalManager.step--;
            }
        }
    }

    public IntegratedCircuit GetIntegratedCircuit(Guid id)
    {
        if (components.ContainsKey(id))
        {
            return components[id];
        }
        return null;
    }

    public void SetNodeConnection(bool isWire, string nodeId, int fromIndex, int toIndex, string id)
    {
        if (nodes.ContainsKey(nodeId))
        {
            nodes[nodeId].SetConnection(new Connection(isWire, id, toIndex), fromIndex);
        }
    }

    public void RemoveNodeConnection(string nodeId, int index)
    {
        if (nodes.ContainsKey(nodeId))
        {
            nodes[nodeId].connections[index] = null;
        }
    }

    public void AddNode(string nodeId, int type)
    {
        if (!nodes.ContainsKey(nodeId))
        {
            nodes.Add(nodeId, new Node(type == 0 ? 63 : 5));
        }
    }

    public int GetNodeState(string nodeId)
    {
        if (nodes.ContainsKey(nodeId))
        {
            return nodes[nodeId].GetState();
        }
        return 0;
    }
    public Vector2Int GetNodeStateFull(string nodeId)
    {
        if (nodes.ContainsKey(nodeId))
        {
            return new Vector2Int(nodes[nodeId].valuePos, nodes[nodeId].valueNeg);
        }
        return new Vector2Int(0, 0);
    }

    public bool NodeHasConnection(string nodeId, int index)
    {
        if (nodes.ContainsKey(nodeId))
        {
            return nodes[nodeId].connections[index] != null;
        }
        return true;
    }

    public bool NodeHasICType(string nodeId, ICType iCType)
    {
        if (nodes.ContainsKey(nodeId))
        {
            Node node = nodes[nodeId];
            foreach(Connection connection in node.connections)
            {
                if (connection != null && !connection.IsNode)
                {
                    Guid id = Guid.Parse(connection.ID);
                    if (!connection.IsNode && components.ContainsKey(id))
                    {
                        IntegratedCircuit c = components[id];
                        if (c.IcType == iCType)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public void PropagateValue(string nodeId, string id, int index, int value)
    {
        PropagateValue(nodeId, id, index, value, 0, 0);
    }

    public void PropagateValue(string nodeId, string id, int index, int value, int posValue, int negValue)
    {
        //Debug.Log("PropagateValue: " + nodeId + " : " + value + ", pos: " + posValue + ", neg: " + negValue);
        int nodeListIndex = 0;
        List<string> nodeList = new List<string>();
        nodeList.Add(nodeId);

        while(nodeListIndex < nodeList.Count)
        {
            //Debug.Log(nodeList[nodeListIndex]);
            Node node = nodes[nodeList[nodeListIndex]];
            nodeListIndex++;

            int oldNeg = node.valueNeg;
            int oldPos = node.valuePos;
            
            switch(value)
            {
                case 2:
                    node.valueNeg++;
                    node.valuePos++;
                    break;
                case -2:
                    node.valueNeg--;
                    node.valuePos--;
                    break;
                case 1:
                    node.valuePos++;
                    break;
                case -1:
                    node.valueNeg--;
                    break;
                case 3:
                    node.valueNeg++;
                    break;
                case -3:
                    node.valuePos--;
                    break;
                case 0:
                    break;
                default:
                    node.valueNeg = negValue;
                    node.valuePos = posValue;
                    break;
            }

            //Debug.Log("Old Pos: " + oldPos.ToString() + ", New Pos: " + node.valuePos.ToString() + ", Old Neg: " + oldNeg.ToString() + ", New Neg: " + node.valueNeg.ToString());

            foreach (Connection connection in node.connections)
            {

                if (connection != null && !connection.ID.Equals(""))
                {                    
                    if (connection.IsNode)
                    {
                        if (!nodeList.Contains(connection.ID))
                        {
                            nodeList.Add(connection.ID);
                        }
                    }
                    else if (value == 0 || (oldPos + node.valuePos == 1) || (oldPos == 0 && node.valuePos == 0 && (oldNeg + node.valueNeg == -1)))
                    {
                        //Debug.Log(connection.ID + " => " + id + ", " + connection.Index.ToString() + " => " + index.ToString());
                        if (connection.ID != id || connection.Index != index)
                        {
                            if (!updates.Contains(Guid.Parse(connection.ID)))
                            {
                                updates.Enqueue(Guid.Parse(connection.ID));
                            }
                        }
                    }
                }
            }
        }
    }

    public void UnlinkNodes(string nodeIdA, int indexA, string nodeIdB, int indexB)
    {
        //Debug.Log("A: " + nodeIdA + ", B: " + nodeIdB);

        RemoveNodeConnection(nodeIdA, indexA);
        RemoveNodeConnection(nodeIdB, indexB);

        if(!AreNodesConnected(nodeIdA, nodeIdB))
        {
            Vector2Int node = NodeValues(nodeIdA);
            PropagateValue(nodeIdA, "", indexA, 100, node.x, node.y);

            node = NodeValues(nodeIdB);
            PropagateValue(nodeIdB, "", indexB, 100, node.x, node.y);

            PropagateValue(nodeIdA, "", indexA, 0);
            PropagateValue(nodeIdB, "", indexB, 0);
        }
        
    }

    public void LinkNodes(string nodeIdA, int indexA, string nodeIdB, int indexB)
    {
        if(AreNodesConnected(nodeIdA, nodeIdB))
        {
            SetNodeConnection(true, nodeIdA, indexA, indexB, nodeIdB);
            SetNodeConnection(true, nodeIdB, indexB, indexA, nodeIdA);
        }
        else
        {
            SetNodeConnection(true, nodeIdA, indexA, indexB, nodeIdB);
            SetNodeConnection(true, nodeIdB, indexB, indexA, nodeIdA);

            int pos = nodes[nodeIdA].valuePos + nodes[nodeIdB].valuePos;
            int neg = nodes[nodeIdA].valueNeg + nodes[nodeIdB].valueNeg;

            PropagateValue(nodeIdA, "", indexA, 100, pos, neg);
            PropagateValue(nodeIdA, "", indexA, 0);            
        }
    }

    private Vector2Int NodeValues(string nodeId)
    {
        Vector2Int result = new Vector2Int(0, 0);

        int nodeListIndex = 0;
        List<string> nodeList = new List<string>();
        nodeList.Add(nodeId);

        while (nodeListIndex < nodeList.Count)
        {
            Debug.Log("Checking: " + nodeList[nodeListIndex]);
            Node node = nodes[nodeList[nodeListIndex]];
            nodeListIndex++;

            foreach (Connection connection in node.connections)
            {
                if (connection != null && !connection.ID.Equals(""))
                {
                    if (connection.IsNode)
                    {
                        if (!nodeList.Contains(connection.ID))
                        {
                            nodeList.Add(connection.ID);
                        }
                    }
                    else
                    {
                        IntegratedCircuit ic = components[Guid.Parse(connection.ID)];
                        int state = ic.GetPinState(connection.Index);
                        if(state == -1)
                        {
                            result.y--;
                        }
                        else if(state == 1)
                        {
                            result.x++;
                        }
                    }
                }
            }

        }

        //Debug.Log("Pos: " + result.x.ToString() + ", Neg: " + result.y.ToString());
        return result;
    }

    private bool AreNodesConnected(string nodeIdA, string nodeIdB)
    {
        int nodeListIndex = 0;
        List<string> nodeList = new List<string>();
        nodeList.Add(nodeIdA);

        while (nodeListIndex < nodeList.Count)
        {
            Node node = nodes[nodeList[nodeListIndex]];
            nodeListIndex++;

            foreach (Connection connection in node.connections)
            {
                if (connection != null && !connection.ID.Equals(""))
                {
                    if (connection.IsNode)
                    {

                        if(connection.ID.Equals(nodeIdB))
                        {
                            return true;
                        }

                        if (!nodeList.Contains(connection.ID))
                        {
                            nodeList.Add(connection.ID);
                        }
                    }                    
                }
            }

        }

        return false;

    }

}

public class Node
{
    public int valuePos = 0;
    public int valueNeg = 0;
    public Connection[] connections;

    public Node(int indexes)
    {
        connections = new Connection[indexes];
    }

    public int GetState()
    {
        if (valuePos > 0)
        {
            return 1;
            //return valuePos;
        }
        if (valueNeg < 0)
        {
            return -1;
            //return valueNeg;
        }
        return 0;
    }

    public void SetConnection(Connection connection, int index)
    {
        connections[index] = connection;
    }

}

public class Connection
{
    public bool IsNode;
    public string ID;
    public int Index;

    public Connection(bool isNode, string id, int index)
    {
        IsNode = isNode;
        ID = id;
        Index = index;
    }
}
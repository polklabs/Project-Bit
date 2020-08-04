using System;
using System.Collections.Generic;
using UnityEngine;
using IntegratedCircuits;
using System.Diagnostics;
using System.Runtime.Serialization;
using UnityEngine.UI;
using System.Linq;

public class BreadBoard : MonoBehaviour
{
    public GeneralManager generalManager;

    public readonly Dictionary<Guid, IntegratedCircuit> components = new Dictionary<Guid, IntegratedCircuit>();
    public readonly Dictionary<string, Node> nodes = new Dictionary<string, Node>();
    public Queue<Guid> updates = new Queue<Guid>();

    public readonly Dictionary<Guid, GameObject> updateOverlays = new Dictionary<Guid, GameObject>();

    public Material stateOn;
    public Material stateOff;

    public GameObject overlayPrefab;
    public Transform overlayParent;

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
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (updates.Count > 0)
            {

                UpdateFromQueue(updates.Dequeue());

                if (stopwatch.ElapsedMilliseconds > 45)
                {
                    UnityEngine.Debug.Log("Break from stopwatch: " + stopwatch.ElapsedMilliseconds);
                    break;
                }

                if (generalManager.paused)
                {
                    break;
                }
            }
            stopwatch.Stop();
        } else
        {
            
            if (generalManager.step > 0)
            {
                generalManager.DecrementStep();

                if (generalManager.step == 0 && updates.Count == 0)
                {
                    generalManager.Step(1);
                }

                if (generalManager.step > 0 && updates.Count > 0)
                {
                    Queue<Guid> stepQueue = new Queue<Guid>(updates);
                    updates.Clear();

                    while (stepQueue.Count > 0)
                    {
                        UpdateFromQueue(stepQueue.Dequeue());
                    }
                }
                SetComponentDisplay();                
            }
            else if(generalManager.singleStep > 0)
            {
                generalManager.DecrementSingleStep();
                if (updates.Count > 0)
                {
                    UpdateFromQueue(updates.Dequeue());
                    SetComponentDisplay();
                }
            }
        }
    }

    private Guid UpdateFromQueue(Guid id)
    {
        if (components.ContainsKey(id))
        {
            components[id].Update();
        }
        return id;
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
        if (nodes.ContainsKey(nodeId) && nodes[nodeId].connections.ContainsKey(index))
        {
            nodes[nodeId].connections.Remove(index);
        }
    }

    public void AddNode(string nodeId, int type, MeshRenderer meshRenderer)
    {
        if (!nodes.ContainsKey(nodeId))
        {
            nodes.Add(nodeId, new Node(type == 0 ? 63 : 5, meshRenderer));
        } else
        {
            nodes[nodeId].addMeshRenderer(meshRenderer);
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
            return nodes[nodeId].connections.ContainsKey(index);
        }
        return true;
    }

    public bool NodeHasICType(string nodeId, ICType iCType)
    {
        if (nodes.ContainsKey(nodeId))
        {
            Node node = nodes[nodeId];
            foreach(Connection connection in node.connections.Values)
            {
                if (!connection.IsNode)
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
        int nodeListIndex = 0;
        List<string> nodeList = new List<string>();
        nodeList.Add(nodeId);
        bool addSelfToQueue = false;

        while(nodeListIndex < nodeList.Count)
        {            

            Node node = nodes[nodeList[nodeListIndex]];
            string nodeUpdateId = nodeList[nodeListIndex];
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

            foreach (Connection connection in node.connections.Values)
            {

                if (!connection.ID.Equals(""))
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
                        Guid ID = Guid.Parse(connection.ID);
                        if (components[ID].GetPinMode(nodeUpdateId) == PinMode.Input)
                        {
                            if (connection.ID != id)
                            {
                                if (!updates.Contains(ID))
                                {
                                    updates.Enqueue(ID);
                                }
                            }
                            else if (connection.Index != index)
                            {
                                addSelfToQueue = true;
                            }
                        }
                    }
                }
            }

            if (generalManager.paused)
            {
                UpdateNodeDisplay(nodeUpdateId);
            }
        }

        // If a component feeds back to itself then add that to the queue last. Otherwise it can cause weird race conditions
        if (addSelfToQueue)
        {
            if (!updates.Contains(Guid.Parse(id)))
            {
                updates.Enqueue(Guid.Parse(id));
            }
        }

    }

    public void UpdateNodeDisplay(string id)
    {
        Node n = nodes[id];
        foreach(MeshRenderer meshRenderer in n.meshRenderers)
        {
            switch (n.GetState())
            {
                case 1:
                    meshRenderer.material = stateOn;
                    meshRenderer.enabled = true;
                    break;
                case -1:
                    meshRenderer.material = stateOff;
                    meshRenderer.enabled = true;
                    break;
                default:
                    meshRenderer.enabled = false;
                    break;
            }
        }        
    }

    public void SetNodeDisplay()
    {
        foreach (string id in nodes.Keys)
        {
            UpdateNodeDisplay(id);            
        }
    }

    public void ClearNodeDisplay()
    {
        foreach(string id in nodes.Keys)
        {
            foreach(MeshRenderer meshRenderer in nodes[id].meshRenderers)
            {
                meshRenderer.enabled = false;
            }                        
        }
    }

    public void SetComponentDisplay()
    {
        if (!generalManager.paused)
        {
            return;
        }

        Guid[] guids = updateOverlays.Keys.ToArray();
        foreach (Guid g in guids)
        {
            if (!updates.Contains(g))
            {
                Destroy(updateOverlays[g]);
                updateOverlays.Remove(g);
            }
        }

        guids = updates.ToArray();
        for (int i = 0; i < guids.Length; i++)
        {
            GameObject overlayObject;
            if (updateOverlays.ContainsKey(guids[i]))
            {
                overlayObject = updateOverlays[guids[i]];
            } else
            {
                Vector3 position;
                
                BoxCollider bc = components[guids[i]].GetObjRef().GetComponentInChildren<BoxCollider>();
                if (bc == null)
                {
                    Vector3 componentPosition = components[guids[i]].GetObjRef().transform.position;
                    position = new Vector3(componentPosition.x, 3, componentPosition.z);
                } else
                {
                    position = bc.transform.TransformPoint(bc.center);
                    position.y = 3;
                }
                
                overlayObject = Instantiate(overlayPrefab, position, Quaternion.Euler(90, 0, 0)) as GameObject;
                overlayObject.transform.SetParent(overlayParent, false);
                updateOverlays[guids[i]] = overlayObject;
            }

            Text text = overlayObject.GetComponentInChildren<Text>();
            text.text = (i + 1).ToString();
        }

    }

    public void ClearComponentDisplay()
    {
        Guid[] guids = updateOverlays.Keys.ToArray();
        foreach (Guid g in guids)
        {
            Destroy(updateOverlays[g]);
            updateOverlays.Remove(g);
        }
    }

    public void UnlinkNodes(string nodeIdA, int indexA, string nodeIdB, int indexB)
    {

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
            Node node = nodes[nodeList[nodeListIndex]];
            nodeListIndex++;

            foreach (Connection connection in node.connections.Values)
            {
                if (!connection.ID.Equals(""))
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

            foreach (Connection connection in node.connections.Values)
            {
                if (!connection.ID.Equals(""))
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

[DataContract]
public class Node
{
    [DataMember]
    public int valuePos = 0;
    [DataMember]
    public int valueNeg = 0;
    [DataMember]
    public Dictionary<int, Connection> connections = new Dictionary<int, Connection>();
    [DataMember]
    public int maxIndex;

    public List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

    public Node(int indexes, MeshRenderer meshRenderer)
    {
        maxIndex = indexes;
        meshRenderers.Add(meshRenderer);
    }

    public void addMeshRenderer(MeshRenderer meshRenderer)
    {
        meshRenderers.Add(meshRenderer);
    }

    public int GetState()
    {
        if (valuePos > 0)
        {
            return 1;
        }
        if (valueNeg < 0)
        {
            return -1;
        }
        return 0;
    }

    public void SetConnection(Connection connection, int index)
    {
        if (index < maxIndex)
        {
            connections[index] = connection;
        }
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
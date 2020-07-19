using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using IntegratedCircuits;
using Helper;
using UnityEngine.UI;

public class Load : MonoBehaviour
{
    public BreadBoard bb;
    public CircuitPool cp;
    public Fabricator fabricator;

    public Text Title;

    private void Start()
    {
        string title = PlayerPrefs.GetString("Workspace");
        LoadWorkspace(title);
        Title.text = title;
    }

    public void LoadWorkspace(string workSpaceName)
    {
        string filePath = GameHelper.GetSaveDirectory();
        filePath += workSpaceName;

        if (Directory.Exists(filePath))
        {
            LoadBreadBoards(filePath);
            LoadUpdates(filePath);
            LoadNodes(filePath);
            LoadComponents(filePath);
        } else
        {
            fabricator.addBoard("0", 0, 0, false);            
        }

    }

    private void LoadComponents(string filePath)
    {
        filePath += "/components.json";
        if (!File.Exists(filePath))
        {
            return;
        }

        using (StreamReader file = File.OpenText(filePath))
        {
            JsonSerializer serializer = new JsonSerializer
            {
                TypeNameHandling = TypeNameHandling.Auto,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                Converters = { new BitArrayConverter(), new Vector3Converter() }
            };
            Dictionary<Guid, IntegratedCircuit> components = (Dictionary<Guid, IntegratedCircuit>)serializer.Deserialize(file, typeof(Dictionary<Guid, IntegratedCircuit>));

            foreach(Guid g in components.Keys)
            {
                //Debug.Log(components[g].GetType().Name);

                IntegratedCircuit ic = components[g];
                GameObject obj;

                switch (ic.IcType)
                {
                    case ICType.solo:

                        obj = cp.PlaceIntegratedCircuit(ic, ic.GetPinNode(0), ic.GetPinNodeIndex(0), true);
                        obj.transform.position = CircuitHelper.GetPositionFromNode(ic.GetPinNode(0), ic.GetPinNodeIndex(0));

                        break;
                    case ICType.dual:

                        obj = cp.PlaceIntegratedCircuit(ic, ic.GetPinNode(0), ic.GetPinNodeIndex(0), ic.GetPinNode(1), ic.GetPinNodeIndex(1), true);

                        Vector3 locationA = CircuitHelper.GetPositionFromNode(ic.GetPinNode(0), ic.GetPinNodeIndex(0));
                        Vector3 locationB = CircuitHelper.GetPositionFromNode(ic.GetPinNode(1), ic.GetPinNodeIndex(1));

                        obj.transform.position = new Vector3((locationA.x + locationB.x) / 2, 0, (locationA.z + locationB.z) / 2);
                        obj.transform.localScale = new Vector3(Vector3.Distance(locationB, locationA) / 2, 1, 1);
                        float angle = CircuitHelper.AngleBetweenVector3(locationB, locationA);
                        angle = locationA.z > locationB.z ? -angle : angle;
                        obj.transform.rotation = Quaternion.Euler(0, angle, 0);

                        break;
                    case ICType.ic4:
                        obj = cp.PlaceIntegratedCircuit(ic, ic.GetPinNode(0), ic.GetPinNodeIndex(0), true);
                        obj.transform.position = CircuitHelper.GetPositionFromNode(ic.GetPinNode(0), ic.GetPinNodeIndex(0));
                        break;
                    case ICType.ic6:
                        obj = cp.PlaceIntegratedCircuit(ic, ic.GetPinNode(0), ic.GetPinNodeIndex(0), true);
                        obj.transform.position = CircuitHelper.GetPositionFromNode(ic.GetPinNode(0), ic.GetPinNodeIndex(0));
                        break;
                    case ICType.wire:

                        obj = cp.PlaceIntegratedCircuit(ic, ic.GetPinNode(0), ic.GetPinNodeIndex(0), ic.GetPinNode(1), ic.GetPinNodeIndex(1), true);

                        Vector3[] vectors = ((Wire)ic).points;

                        LineRenderer lineRenderer = obj.GetComponentInChildren<LineRenderer>();
                        lineRenderer.positionCount = vectors.Length;
                        lineRenderer.SetPositions(vectors);

                        CircuitHelper.CreateColliderChain(obj, vectors);

                        break;
                    default:
                        break;
                }

                ic.CustomMethod();

            }

        }
    }    

    private void LoadNodes(string filePath)
    {
        filePath += "/nodes.json";
        if (!File.Exists(filePath))
        {
            return;
        }

        using (StreamReader file = File.OpenText(filePath))
        {
            JsonSerializer serializer = new JsonSerializer();
            Dictionary<string, Node> nodes = (Dictionary<string, Node>)serializer.Deserialize(file, typeof(Dictionary<string, Node>));

            bb.nodes = nodes;

        }
    }

    private void LoadUpdates(string filePath)
    {
        filePath += "/updates.json";
        if (!File.Exists(filePath))
        {
            return;
        }

        using (StreamReader file = File.OpenText(filePath))
        {
            JsonSerializer serializer = new JsonSerializer();
            Queue<Guid> updates = (Queue<Guid>)serializer.Deserialize(file, typeof(Queue<Guid>));

            bb.updates = updates;

        }
    }

    private void LoadBreadBoards(string filePath)
    {
        filePath += "/breadboard.json";
        if (!File.Exists(filePath))
        {
            return;
        }

        using (StreamReader file = File.OpenText(filePath))
        {
            JsonSerializer serializer = new JsonSerializer();
            fabricator.breadBoardData = (List<BreadBoardData>)serializer.Deserialize(file, typeof(List<BreadBoardData>));
            fabricator.loadFromBreadBoardData();
        }
    }

}

﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using IntegratedCircuits;
using Helper;

public class Load : MonoBehaviour
{
    public BreadBoard bb;
    public CircuitPool cp;

    private void Start()
    {
       LoadWorkspace("Test Workspace");
    }

    public void LoadWorkspace(string workSpaceName)
    {
#if UNITY_STANDALONE_WIN
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/");
        filePath += "/My Games/Project Logical/";
#else
        string filePath = Application.persistentDataPath + "/";
#endif
        filePath += workSpaceName;

        if (Directory.Exists(filePath))
        {
            LoadUpdates(filePath);
            LoadNodes(filePath);
            LoadComponents(filePath);
        }

    }

    private void LoadComponents(string filePath)
    {
        filePath += "/components.json";
        using (StreamReader file = File.OpenText(filePath))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Auto;
            Dictionary<Guid, IntegratedCircuit> components = (Dictionary<Guid, IntegratedCircuit>)serializer.Deserialize(file, typeof(Dictionary<Guid, IntegratedCircuit>));

            foreach(Guid g in components.Keys)
            {
                //Debug.Log(components[g].GetType().Name);

                IntegratedCircuit ic = components[g];
                GameObject obj;

                switch (ic.IcType)
                {
                    case ICType.solo:

                        obj = cp.PlaceIntegratedCircuit(ic, ic.GetType().Name, ic.GetPinNode(0), ic.GetPinNodeIndex(0));
                        obj.transform.position = CircuitHelper.GetPositionFromNode(ic.GetPinNode(0), ic.GetPinNodeIndex(0));

                        break;
                    case ICType.dual:

                        obj = cp.PlaceIntegratedCircuit(ic, ic.GetType().Name, ic.GetPinNode(0), ic.GetPinNodeIndex(0), ic.GetPinNode(1), ic.GetPinNodeIndex(1));

                        Vector3 locationA = CircuitHelper.GetPositionFromNode(ic.GetPinNode(0), ic.GetPinNodeIndex(0));
                        Vector3 locationB = CircuitHelper.GetPositionFromNode(ic.GetPinNode(1), ic.GetPinNodeIndex(1));

                        obj.transform.position = new Vector3((locationA.x + locationB.x) / 2, 0, (locationA.z + locationB.z) / 2);
                        obj.transform.localScale = new Vector3(Vector3.Distance(locationB, locationA) / 2, 1, 1);
                        obj.transform.Rotate(0, CircuitHelper.AngleBetweenVector3(locationA, locationB), 0);

                        break;
                    case ICType.ic4:
                        obj = cp.PlaceIntegratedCircuit(ic, ic.GetType().Name, ic.GetPinNode(0), ic.GetPinNodeIndex(0));
                        obj.transform.position = CircuitHelper.GetPositionFromNode(ic.GetPinNode(0), ic.GetPinNodeIndex(0));
                        break;
                    case ICType.ic6:
                        obj = cp.PlaceIntegratedCircuit(ic, ic.GetType().Name, ic.GetPinNode(0), ic.GetPinNodeIndex(0));
                        obj.transform.position = CircuitHelper.GetPositionFromNode(ic.GetPinNode(0), ic.GetPinNodeIndex(0));
                        break;
                    case ICType.wire:

                        obj = cp.PlaceIntegratedCircuit(ic, ic.GetType().Name, ic.GetPinNode(0), ic.GetPinNodeIndex(0), ic.GetPinNode(1), ic.GetPinNodeIndex(1));

                        locationA = CircuitHelper.GetPositionFromNode(ic.GetPinNode(0), ic.GetPinNodeIndex(0));
                        locationB = CircuitHelper.GetPositionFromNode(ic.GetPinNode(1), ic.GetPinNodeIndex(1));

                        obj.transform.position = new Vector3((locationA.x + locationB.x) / 2, 0, (locationA.z + locationB.z) / 2);
                        obj.transform.localScale = new Vector3(Vector3.Distance(locationB, locationA) / 2, 1, 1);
                        obj.transform.Rotate(0, CircuitHelper.AngleBetweenVector3(locationA, locationB), 0);

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
        using (StreamReader file = File.OpenText(filePath))
        {
            JsonSerializer serializer = new JsonSerializer();
            Queue<Guid> updates = (Queue<Guid>)serializer.Deserialize(file, typeof(Queue<Guid>));

            bb.updates = updates;

        }
    }

}
﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using IntegratedCircuits;
using Helper;
using UnityEngine.UI;
using System.IO.Compression;

public class Load : MonoBehaviour
{
    public GeneralManager gm;

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
        filePath += workSpaceName + ".bit";
        
        if (File.Exists(filePath))
        {
            using (ZipArchive archive = ZipFile.OpenRead(filePath))
            {
                LoadBreadBoards(archive);
                LoadUpdates(archive);
                LoadNodes(archive);
                LoadComponents(archive);
                LoadGeneral(archive);
            }
        }
        else
        {
            fabricator.addBoard("0", 0, 0, false);
        }

    }

    private int GetZipArchiveEntryIndex(string name, ZipArchive archive)
    {
        for(int i = 0; i < archive.Entries.Count; i++)
        {
            if (archive.Entries[i].Name == name)
            {
                return i;
            }
        }
        return -1;
    }

    private void LoadComponents(ZipArchive archive)
    {
        int index = GetZipArchiveEntryIndex("components.json", archive);               
        if (index == -1)
        {
            return;
        }

        ZipArchiveEntry entry = archive.Entries[index];

        using (StreamReader file = new StreamReader(entry.Open()))
        {
            JsonSerializer serializer = new JsonSerializer
            {
                TypeNameHandling = TypeNameHandling.Auto,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                Converters = { new BitArrayConverter(), new Vector3Converter(), new WireConverter() }
            };
            Dictionary<Guid, IntegratedCircuit> components = (Dictionary<Guid, IntegratedCircuit>)serializer.Deserialize(file, typeof(Dictionary<Guid, IntegratedCircuit>));

            foreach (Guid g in components.Keys)
            {

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

    private void LoadNodes(ZipArchive archive)
    {
        int index = GetZipArchiveEntryIndex("nodes.json", archive);
        if (index == -1)
        {
            return;
        }

        ZipArchiveEntry entry = archive.Entries[index];

        using (StreamReader file = new StreamReader(entry.Open()))
        {
            JsonSerializer serializer = new JsonSerializer();
            Dictionary<string, Node> nodes = (Dictionary<string, Node>)serializer.Deserialize(file, typeof(Dictionary<string, Node>));

            foreach (string s in nodes.Keys)
            {
                List<MeshRenderer> tempRenderer = bb.nodes[s].meshRenderers;
                bb.nodes[s] = nodes[s];
                bb.nodes[s].meshRenderers = tempRenderer;
            }
        }
    }

    private void LoadUpdates(ZipArchive archive)
    {
        int index = GetZipArchiveEntryIndex("updates.json", archive);
        if (index == -1)
        {
            return;
        }

        ZipArchiveEntry entry = archive.Entries[index];

        using (StreamReader file = new StreamReader(entry.Open()))
        {
            JsonSerializer serializer = new JsonSerializer();
            Queue<Guid> updates = (Queue<Guid>)serializer.Deserialize(file, typeof(Queue<Guid>));

            bb.updates = updates;
        }
    }

    private void LoadBreadBoards(ZipArchive archive)
    {
        int index = GetZipArchiveEntryIndex("breadboard.json", archive);
        if (index == -1)
        {
            return;
        }

        ZipArchiveEntry entry = archive.Entries[index];

        using (StreamReader file = new StreamReader(entry.Open()))
        {
            JsonSerializer serializer = new JsonSerializer
            {
                Converters = { new BreadBoardConverter() }
            };
            fabricator.breadBoardData = (List<BreadBoardData>)serializer.Deserialize(file, typeof(List<BreadBoardData>));
            fabricator.loadFromBreadBoardData();
        }
    }

    private void LoadGeneral(ZipArchive archive)
    {
        int index = GetZipArchiveEntryIndex("general.json", archive);
        if (index == -1)
        {
            return;
        }

        ZipArchiveEntry entry = archive.Entries[index];

        using (StreamReader file = new StreamReader(entry.Open()))
        {
            JsonSerializer serializer = new JsonSerializer
            {
                Converters = { new Vector3Converter() }
            };
            gm.CopyStruct((GeneralManagerStruct)serializer.Deserialize(file, typeof(GeneralManagerStruct)));
        }
    }

}

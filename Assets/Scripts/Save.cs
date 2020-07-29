﻿using Newtonsoft.Json;
using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class Save : MonoBehaviour
{

    public BreadBoard bb;
    public Fabricator fabricator;

    public void SaveWorkspaceGUI()
    {
        if (SaveWorkspace())
        {
            Debug.Log("Saved Workspace!");
        }
    }

    public bool SaveWorkspace()
    {
        string filePath = Helper.GameHelper.GetSaveDirectory();
        filePath += PlayerPrefs.GetString("Workspace");
        Directory.CreateDirectory(filePath);

        SaveComponents(filePath);
        SaveNodes(filePath);
        SaveUpdates(filePath);
        SaveBreadBoards(filePath);

        return true;

    }

    private void SaveComponents(string filePath)
    {
        filePath += "/components.json";

        using(StreamWriter file = File.CreateText(filePath))
        {
            JsonSerializer serializer = new JsonSerializer
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented,
                Converters = { new BitArrayConverter(), new Vector3Converter(), new WireDictionaryConverter() }
            };            

            serializer.Serialize(file, bb.components);
        }
    }

    private void SaveNodes(string filePath)
    {
        filePath += "/nodes.json";

        Dictionary<string, Node> nodeDict = new Dictionary<string, Node>();

        foreach (string key in bb.nodes.Keys)
        {
            if (bb.nodes[key].connections.Count > 0)
            {
                nodeDict.Add(key, bb.nodes[key]);
            }
        }

        using (StreamWriter file = File.CreateText(filePath))
        {
            JsonSerializer serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented
            };

            serializer.Serialize(file, nodeDict);
        }
    }

    private void SaveUpdates(string filePath)
    {
        filePath += "/updates.json";

        using (StreamWriter file = File.CreateText(filePath))
        {
            JsonSerializer serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented
            };

            serializer.Serialize(file, bb.updates);
        }
    }

    private void SaveBreadBoards(string filePath)
    {
        filePath += "/breadboard.json";

        using (StreamWriter file = File.CreateText(filePath))
        {
            JsonSerializer serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented
            };

            serializer.Serialize(file, fabricator.breadBoardData);
        }
    }
}

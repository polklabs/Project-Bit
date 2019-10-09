﻿using Newtonsoft.Json;
using System;
using UnityEngine;
using System.IO;

public class Save : MonoBehaviour
{

    public BreadBoard bb;

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
                Formatting = Formatting.Indented
            };            

            serializer.Serialize(file, bb.components);
        }
    }

    private void SaveNodes(string filePath)
    {
        filePath += "/nodes.json";

        using (StreamWriter file = File.CreateText(filePath))
        {
            JsonSerializer serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented
            };

            serializer.Serialize(file, bb.nodes);
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
}

using Newtonsoft.Json;
using System;
using UnityEngine;
using System.IO;

public class Save : MonoBehaviour
{

    public BreadBoard bb;

    public void SaveWorkspace(string workSpaceName)
    {
#if UNITY_STANDALONE_WIN
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/");       
        filePath += "/My Games/Project Logical/";        
#else
        string filePath = Application.persistentDataPath + "/";
#endif

        filePath += workSpaceName;
        Directory.CreateDirectory(filePath);

        SaveComponents(filePath);
        SaveNodes(filePath);
        SaveUpdates(filePath);

        Debug.Log("Workspace Saved!");

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

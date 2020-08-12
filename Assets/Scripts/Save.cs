using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.IO.Compression;

public class Save : MonoBehaviour
{
    public GeneralManager gm;

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
        filePath += PlayerPrefs.GetString("Workspace") + ".bit";

        using (ZipArchive archive = ZipFile.Open(filePath, ZipArchiveMode.Update))
        {
            SaveComponents(archive);
            SaveNodes(archive);
            SaveUpdates(archive);
            SaveBreadBoards(archive);
            SaveGeneral(archive);
        }

        return true;

    }

    private ZipArchiveEntry GetZipArchiveEntry(string name, ZipArchive archive)
    {
        for (int i = 0; i < archive.Entries.Count; i++)
        {
            if (archive.Entries[i].Name == name)
            {
                return archive.Entries[i];
            }
        }
        return archive.CreateEntry(name);
    }

    private void SaveComponents(ZipArchive archive)
    {
        ZipArchiveEntry entry = GetZipArchiveEntry("components.json", archive);

        using(StreamWriter file = new StreamWriter(entry.Open()))
        {
            JsonSerializer serializer = new JsonSerializer
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.None,
                Converters = { new BitArrayConverter(), new Vector3Converter(), new WireConverter() }
            };            

            serializer.Serialize(file, bb.components);
        }
    }

    private void SaveNodes(ZipArchive archive)
    {
        ZipArchiveEntry entry = GetZipArchiveEntry("nodes.json", archive);

        Dictionary<string, Node> nodeDict = new Dictionary<string, Node>();

        foreach (string key in bb.nodes.Keys)
        {
            if (bb.nodes[key].connections.Count > 0)
            {
                nodeDict.Add(key, bb.nodes[key]);
            }
        }

        using (StreamWriter file = new StreamWriter(entry.Open()))
        {
            JsonSerializer serializer = new JsonSerializer
            {
                Formatting = Formatting.None
            };

            serializer.Serialize(file, nodeDict);
        }
    }

    private void SaveUpdates(ZipArchive archive)
    {
        ZipArchiveEntry entry = GetZipArchiveEntry("updates.json", archive);        

        using (StreamWriter file = new StreamWriter(entry.Open()))
        {
            JsonSerializer serializer = new JsonSerializer
            {
                Formatting = Formatting.None
            };

            serializer.Serialize(file, bb.updates);
        }
    }

    private void SaveBreadBoards(ZipArchive archive)
    {
        ZipArchiveEntry entry = GetZipArchiveEntry("breadboard.json", archive);        

        using (StreamWriter file = new StreamWriter(entry.Open()))
        {
            JsonSerializer serializer = new JsonSerializer
            {
                Formatting = Formatting.None,
                Converters = { new BreadBoardConverter() }
            };

            serializer.Serialize(file, fabricator.breadBoardData);
        }
    }

    private void SaveGeneral(ZipArchive archive)
    {
        ZipArchiveEntry entry = GetZipArchiveEntry("general.json", archive);        

        using (StreamWriter file = new StreamWriter(entry.Open()))
        {
            JsonSerializer serializer = new JsonSerializer
            {
                Formatting = Formatting.None,
                Converters = { new Vector3Converter() }
            };

            serializer.Serialize(file, gm.GenerateStruct());
        }
    }
}

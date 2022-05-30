using Helper;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadMenuManager : Singleton<LoadMenuManager>
{
    public Transform LoadContent;
    public GameObject LoadButtonTemplate;

    private string baseFilePath;

    public void Start()
    {
        baseFilePath = GameHelper.GetSaveDirectory();
        LoadLoadMenu();
    }

    public void LoadLoadMenu()
    {
        for (int i = 0; i < LoadContent.childCount; i++)
        {
            Destroy(LoadContent.GetChild(i).gameObject);
        }

        string[] dirs = Directory.GetFiles(baseFilePath, "*.bit", SearchOption.TopDirectoryOnly);
        foreach (string dir in dirs)
        {
            string[] path = dir.Split('/');
            string name = path[path.Length - 1].Substring(0, path[path.Length - 1].LastIndexOf('.'));

            GameObject obj = Instantiate(LoadButtonTemplate, LoadContent) as GameObject;
            obj.GetComponentInChildren<Text>().text = name;
            obj.GetComponent<Button>().onClick.AddListener(delegate { LoadWorkspace(name); });
            obj.name = name;
        }
    }

    public void LoadWorkspace(string name)
    {
        PlayerPrefs.SetString("Workspace", name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

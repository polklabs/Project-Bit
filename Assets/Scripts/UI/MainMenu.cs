using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Helper;

public class MainMenu : MonoBehaviour
{    
    public GameObject NewGameObj;    
    public GameObject LoadGameObj;
    public GameObject LoadedGameObj;

    public Text TitleText;
    public Text NewWorkspaceTitle;

    public GameObject TemplateButton;

    private string baseFilePath;

    private void Start()
    {
        Application.targetFrameRate = 30;
        baseFilePath = GameHelper.GetSaveDirectory();
    }

    public void LoadNewMenu()
    {
        UnloadAll();
        NewGameObj.SetActive(true);
    }

    public void LoadLoadMenu()
    {
        UnloadAll();
        LoadGameObj.SetActive(true);

        for(int i = 1; i < TemplateButton.transform.parent.childCount; i++)
        {
            Destroy(TemplateButton.transform.parent.GetChild(i));
        }

        string[] dirs = Directory.GetFiles(baseFilePath, "*.bit", SearchOption.TopDirectoryOnly);
        foreach(string dir in dirs)
        {
            string[] path = dir.Split('/');
            string name = path[path.Length - 1].Substring(0, path[path.Length - 1].LastIndexOf('.'));

            GameObject obj = Instantiate(TemplateButton, TemplateButton.transform.parent) as GameObject;
            obj.GetComponentInChildren<Text>().text = name;
            obj.GetComponent<Button>().onClick.AddListener(delegate { ClickSavedWorkspace(name); });
            obj.name = name;
            obj.SetActive(true);
        }
    }

    public void ClickSavedWorkspace(string name)
    {
        TitleText.text = name;
        PlayerPrefs.SetString("Workspace", name);
        LoadedGameObj.SetActive(true);
    }

    public void NewWorkspace()
    {        
        PlayerPrefs.SetString("Workspace", NewWorkspaceTitle.text);
        GoToWorkspace();
    }

    public void GoToWorkspace()
    {
        Debug.Log(PlayerPrefs.GetString("Workspace"));
        SceneManager.LoadScene("Tron", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void UnloadAll()
    {
        NewGameObj.SetActive(false);
        LoadGameObj.SetActive(false);
        LoadedGameObj.SetActive(false);
    }

}

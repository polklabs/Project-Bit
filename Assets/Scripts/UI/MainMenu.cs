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
    public GameObject MainMenuObj;
    public GameObject LoadGameObj;
    public GameObject LoadedGameObj;

    public Text TitleText;
    public Text NewWorkspaceTitle;

    public GameObject TemplateButton;

    private string baseFilePath;

    private void Start()
    {
        baseFilePath = Helper.GameHelper.GetSaveDirectory();
    }

    public void LoadNewMenu()
    {
        UnloadAll();
        NewGameObj.SetActive(true);
    }

    public void LoadMainMenu()
    {
        UnloadAll();        
        MainMenuObj.SetActive(true);        
    }

    public void LoadLoadMenu()
    {
        UnloadAll();
        LoadGameObj.SetActive(true);

        string[] dirs = Directory.GetDirectories(baseFilePath, "*", SearchOption.TopDirectoryOnly);
        foreach(string dir in dirs)
        {
            string[] path = dir.Split('/');

            GameObject obj = Instantiate(TemplateButton, TemplateButton.transform.parent) as GameObject;
            obj.GetComponentInChildren<Text>().text = path[path.Length - 1];
            obj.GetComponent<Button>().onClick.AddListener(delegate { ClickSavedWorkspace(path[path.Length - 1]); });
            obj.name = path[path.Length - 1];
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
        MainMenuObj.SetActive(false);
        LoadGameObj.SetActive(false);
        LoadedGameObj.SetActive(false);
    }

}

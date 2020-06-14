using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using System;

[Serializable]
public class SceneInformation
{
    public string name;
    public int index;
}
public class MainMenuButtons : MonoBehaviour
{

    [Header("Play Button")]
    public int SceneIndex = 0;
    //List<string> scenes = new List<string>();
    public List<SceneInformation> MySceneList = new List<SceneInformation>();

    private void Start()
    {
        MySceneList.Clear();
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; ++i)
        {
            string name = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            int index = SceneUtility.GetBuildIndexByScenePath(name);
            SceneInformation tempScene = new SceneInformation();
            tempScene.name = name;
            tempScene.index = index;
            MySceneList.Add(tempScene);
        }

    }
    public void OnPlayClick()
    {
        SceneManager.LoadScene(SceneIndex);


    }
    public void OnExitClick()
    {
        Application.Quit();
    }
    public void OnOptionsClick(GameObject options)
    {
        if (options.activeSelf)
        {
            options.SetActive(false);
        }
        else
        { 
            options.SetActive(true);
        }
    }

}

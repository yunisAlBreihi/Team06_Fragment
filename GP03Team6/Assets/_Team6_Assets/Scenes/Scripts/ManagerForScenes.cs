using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerForScenes : MonoBehaviour
{
    public static ManagerForScenes Instance { set; get; }
    public List<string> StartLevels = new List<string>();
    public string PlayerLevel;
    public static PlayerMovement playerMovement;
    void Awake()
    {
        Instance = this;
        playerMovement = FindObjectOfType<PlayerMovement>();
        for (int i = 0; i < StartLevels.Count; i++)
        {
            if(i == 0)
            {
                SceneManager.LoadScene(StartLevels[i], LoadSceneMode.Additive);
                continue;
            }
            LoadFirstScene(StartLevels[i]);
        }
        LoadFirstScene(PlayerLevel);
        GameObject[] p_lay = SceneManager.GetSceneByName(PlayerLevel).GetRootGameObjects();
        for (int i = 0; i < p_lay.Length; i++)
        {
            if (p_lay[i].GetComponent<PlayerMovement>())
            {
                playerMovement = p_lay[i].GetComponent<PlayerMovement>();
            }
        }
        
    }
    public void Load(string sceneName)
    {
        if(!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
           SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
         
        }
    }
    public void Unload(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }
    private void LoadFirstScene(string sceneName)
    {
        if(sceneName != "")
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }
}

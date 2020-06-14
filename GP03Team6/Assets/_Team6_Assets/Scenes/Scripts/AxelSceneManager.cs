using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AxelSceneManager : MonoBehaviour
{
    public int[] scenesToLoad;
    public int[] scenesToUnLoad;


    public void LoadScenes()
    {
        foreach(int item in scenesToUnLoad)
        {
            SceneManager.UnloadSceneAsync(scenesToUnLoad[item]);
        }

        foreach (int item in scenesToLoad)
        {
            SceneManager.LoadSceneAsync(scenesToUnLoad[item], LoadSceneMode.Additive);
        }

        Destroy(this.gameObject);

    }



}

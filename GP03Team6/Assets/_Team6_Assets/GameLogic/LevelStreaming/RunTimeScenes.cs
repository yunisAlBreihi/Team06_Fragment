using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class RunTimeScenes : MonoBehaviour
{
    [Header("Level Two Scene Indexes")]
    public int[] indexes;
    private bool b = true;

#if (UNITY_STANDALONE && !UNITY_EDITOR)

    private void Start()
    {
        SpawnScenes();
    }

    public void SpawnScenes()
    {
        if (b)
        {
            foreach (int item in indexes)
            {
                SceneManager.LoadScene(item, LoadSceneMode.Additive);
            }
            b = false;
        }

    }

#endif

}

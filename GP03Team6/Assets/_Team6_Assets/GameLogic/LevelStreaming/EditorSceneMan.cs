
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if (UNITY_EDITOR) 
using UnityEditor;
using UnityEditor.SceneManagement;

public class EditorSceneMan : MonoBehaviour
{
    [Header("Level Two Scene Paths")]
    public string[] paths;
    private bool b = true;

    public void DrawGizmos()
    {
        SpawnScenes();
    }

    public void SpawnScenes()
    {
        if (b)
        {
            foreach (string item in paths)
            {
                EditorSceneManager.OpenScene(item, OpenSceneMode.Additive);
            }
            b = false;
        }

    }


}
#endif
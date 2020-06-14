using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if (UNITY_EDITOR) 

[CustomEditor(typeof(EditorSceneMan))]
public class SpawnButton : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorSceneMan s = (EditorSceneMan)target;

        if (GUILayout.Button("Load Sublevels"))
        {
            s.SpawnScenes();


        }





    }








}
#endif
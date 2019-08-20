using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveSystem))]
public class SaveSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SaveSystem theSaveSystem = (SaveSystem)target;
        if (GUILayout.Button("Delete Save File", GUILayout.Width(200), GUILayout.Height(40)))
        {
            theSaveSystem.DeleteSaveFile();
        }
    }
     
}

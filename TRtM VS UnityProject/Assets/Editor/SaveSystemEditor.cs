using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveSystem))]
public class SaveSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SaveSystem theSaveSystem = (SaveSystem)target;

        var oldColor = GUI.backgroundColor;

        if (File.Exists(Const.SavePath))
        {
            ColorUtility.TryParseHtmlString("#56C069", out Color color);
            GUI.backgroundColor = color;
        }

        if (GUILayout.Button("Delete Save File", GUILayout.Width(200), GUILayout.Height(40)))
        {
            theSaveSystem.DeleteSaveFile();
        }
        
        GUI.backgroundColor = oldColor;
    }

}

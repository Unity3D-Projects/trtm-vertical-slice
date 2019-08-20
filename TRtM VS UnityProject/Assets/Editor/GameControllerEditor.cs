using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameController))]
public class GameControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var oldColor = GUI.backgroundColor;

        GameController theController = (GameController)target;

        theController.textSpeed = (TextSpeed)EditorGUILayout.EnumPopup("Text speed", theController.textSpeed);
        theController.clampSpeedToOne = EditorGUILayout.Toggle("Clamp speed to one", theController.clampSpeedToOne);
        theController.instantMode = EditorGUILayout.Toggle("Instant mode", theController.instantMode);
        EditorGUILayout.Space();

        theController.AllowRewinding = EditorGUILayout.Toggle("Allow rewinding", theController.AllowRewinding);
        theController.PlayerStandBy = EditorGUILayout.Toggle("Player stand by", theController.PlayerStandBy);
        theController.GameEnded = EditorGUILayout.Toggle("Game ended", theController.GameEnded);
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();

        theController.minutesToSkip = EditorGUILayout.IntField("Minutes to skip", theController.minutesToSkip);

        ColorUtility.TryParseHtmlString("#C55A5A", out Color color);
        GUI.backgroundColor = color;

        if (GUILayout.Button("Skip delay >>", GUILayout.Width(200), GUILayout.Height(40)))
        {
            theController.SkipDelay(theController.minutesToSkip);
        }
        GUILayout.EndHorizontal();

        GUI.backgroundColor = oldColor;
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }
}

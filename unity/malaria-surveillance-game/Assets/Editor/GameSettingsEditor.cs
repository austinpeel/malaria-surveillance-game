using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameSettings))]
public class GameSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GameSettings settings = (GameSettings)target;

        DrawDefaultInspector();

        GUILayout.BeginVertical();
        GUILayout.Space(10);
        GUILayout.EndVertical();
        if (GUILayout.Button("Apply"))
        {
            settings.ApplySettings();
        }
    }
}

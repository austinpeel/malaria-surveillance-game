using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameInitializer))]
public class GameInitializerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GameInitializer gameInitializer = (GameInitializer)target;

        DrawDefaultInspector();

        GUILayout.BeginVertical();
        GUILayout.Space(10);
        GUILayout.EndVertical();
        GUILayout.Label("Options", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        gameInitializer.setRandomSeed = GUILayout.Toggle(gameInitializer.setRandomSeed, " Set Random Seed");
        if (gameInitializer.setRandomSeed)
        {
            gameInitializer.randomSeed = EditorGUILayout.IntField(gameInitializer.randomSeed);
        }
        GUILayout.EndHorizontal();
    }
}

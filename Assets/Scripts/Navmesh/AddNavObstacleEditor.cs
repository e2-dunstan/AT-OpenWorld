using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AddNavObstacle))]
public class AddNavObstacleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AddNavObstacle addNavObstacle = (AddNavObstacle)target;
        if (GUILayout.Button("Generate"))
        {
            addNavObstacle.Generate();
        }
        if (GUILayout.Button("Remove"))
        {
            addNavObstacle.Remove();
        }
        DrawDefaultInspector();
    }
}

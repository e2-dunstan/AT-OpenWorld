using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ModifyCityPrefabs))]
public class ModifyCityPrefabsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ModifyCityPrefabs modifyCityPrefabs = (ModifyCityPrefabs)target;
        if (GUILayout.Button("Generate"))
        {
            modifyCityPrefabs.Generate();
        }
        if (GUILayout.Button("Remove"))
        {
            modifyCityPrefabs.Remove();
        }
        if (GUILayout.Button("Apply Prefabs"))
        {
            modifyCityPrefabs.ApplyPrefabs();
        }
        if (GUILayout.Button("Set Colliders Convex"))
        {
            modifyCityPrefabs.SetCollidersConvex();
        }
        if (GUILayout.Button("Set Colliders Concave"))
        {
            modifyCityPrefabs.SetCollidersConcave();
        }
        DrawDefaultInspector();
    }
}

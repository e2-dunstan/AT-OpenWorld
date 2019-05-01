using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ReadCSV))]
public class ReadCSVEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ReadCSV generator = (ReadCSV)target;
        if (GUILayout.Button("Generate text file"))
        {
            generator.GenerateTextFile();
        }
        DrawDefaultInspector();
    }
}

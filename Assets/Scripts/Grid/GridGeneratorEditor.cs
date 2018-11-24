using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridGenerator))]
public class GridGeneratorEditor : Editor
{
	public override void OnInspectorGUI ()
    {
        GridGenerator gridGenerator = (GridGenerator)target;
        if (GUILayout.Button("Generate"))
        {
            //gridGenerator.Generate();
            Debug.Log("Button disabled: variables are not persistent once entering play mode. To do so " +
                "would involve serialisation which has issues of its own such as not being able to save" +
                "game objects");
        }
        if (GUILayout.Button("Clear"))
        {
            //gridGenerator.Clear();
            Debug.Log("Button disabled: if the grid is cleared now, it will destroy all of the objects within each tile");
        }
        DrawDefaultInspector();
    }
}

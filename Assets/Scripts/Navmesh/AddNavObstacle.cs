using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class AddNavObstacle : MonoBehaviour
{
    public void Generate()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();

        foreach(Transform child in allChildren)
        {
            if (PrefabUtility.GetCorrespondingObjectFromSource(child.gameObject) != null 
                && PrefabUtility.GetPrefabObject(child) != null)
            {
                NavMeshObstacle navMeshObstacle = child.gameObject.AddComponent<NavMeshObstacle>();
                navMeshObstacle.carving = true;
            }
        }
    }

    public void Remove()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();

        foreach (Transform child in allChildren)
        {
            if (child.gameObject.GetComponent<NavMeshObstacle>() != null)
            {
                Destroy(child.gameObject.GetComponent<NavMeshObstacle>());
            }
        }
    }
}

﻿using System.Collections;
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
        NavMeshObstacle[] allChildren = GetComponentsInChildren<NavMeshObstacle>();

        foreach (NavMeshObstacle child in allChildren)
        {
            Destroy(child.gameObject.GetComponent<NavMeshObstacle>());
        }
    }

    public void ApplyPrefabs()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();

        foreach (Transform child in allChildren)
        {
            PrefabUtility.ReplacePrefab(child.gameObject, PrefabUtility.GetCorrespondingObjectFromSource(child.gameObject), 
                ReplacePrefabOptions.ConnectToPrefab);
        }
    }
}

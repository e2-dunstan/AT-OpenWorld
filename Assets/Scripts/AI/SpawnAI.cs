﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAI : MonoBehaviour
{
    private AIContainer aiContainer;
    private List<AI> NPCs;

    public GameObject friendly;
    public GameObject enemy;

    public bool destroyRatherThanDisable = false;

    public int friendliesPerEnemy = 10;

    private void Awake()
    {
        NPCs = new List<AI>();

        string friendlyPath = GetFilePath(friendly);
        string enemyPath = GetFilePath(enemy);

        foreach (ReadCSV.CSVData dataItem in GetComponent<ReadCSV>().ReadFile())
        {
            if (dataItem.count == 0)
                continue;

            for (int i = 0; i < dataItem.count; i++)
            {
                //Spawn enemies
                if (i < dataItem.count / friendliesPerEnemy)
                {
                    AI enemyAI = new AI();
                    enemyAI.SetVariables(enemyPath, new Vector2(dataItem.x, dataItem.y), AI.AIType.ENEMY);
                    NPCs.Add(enemyAI);
                }
                //Spawn friendlies
                else
                {
                    AI friendlyAI = new AI();
                    friendlyAI.SetVariables(friendlyPath, new Vector2(dataItem.x, dataItem.y), AI.AIType.FRIENDLY);
                    NPCs.Add(friendlyAI);
                }
            }
        }
    }

    public void SpawnAIAtTile(Vector2 coord, bool completeReset = true)
    {
        StartCoroutine(SpawnCoroutine(coord, completeReset));
    }
    
    public void DespawnAIAtTile(Vector2 coord)
    {
        StartCoroutine(DespawnCoroutine(coord));
    }

    public void SaveObjects()
    {
        aiContainer = new AIContainer();
        aiContainer.ai = NPCs;
        aiContainer.Save("Assets/Resources/ai.xml");
    }
    private string GetFilePath(GameObject _obj)
    {
        string path = UnityEditor.AssetDatabase.GetAssetPath(_obj);

        string resourcePath = "Assets/Resources/";
        if (path.Contains(resourcePath))
        {
            path = path.Replace(resourcePath, "");
        }

        string[] splitString = path.Split('.');
        path = splitString[0];

        return path;
    }

    public Vector3 GetRandomSpawnPosition(Vector2 coord)
    {
        Vector2 length = new Vector2(AStarGrid.g.gridSize.x, AStarGrid.g.gridSize.y);
        Node randNode = AStarGrid.g.grid[0, 0];

        bool invalid = true;

        int safetyCheck = 0;

        while (invalid)
        {
            safetyCheck++;
            if (safetyCheck > 1000)
            {
                Debug.LogWarning("Spawn location not found in tile " + coord + ". Please modify aiCount.csv.");
                return Vector3.zero;
            }
            randNode = AStarGrid.g.grid[(int)Random.Range(1, length.x - 1), 
                                        (int)Random.Range(1, length.y - 1)];

            if (randNode.walkable && randNode.locationInStreamingGrid == coord)
            {
                invalid = false;
            }
        }

        return randNode.worldPosition;
    }

    private IEnumerator SpawnCoroutine(Vector2 coord, bool completeReset)
    {
        int iterations = 0;
        int maxInterationsPerFrame = 1;

        //PathRequestManager.prm.ClearQueue();
        foreach (AI npc in NPCs)
        {
            if (npc.coordinate == coord)
            {
                GameObject newNPC = null;
                if (completeReset)
                {
                    if (npc.obj != null)
                    {
                        npc.obj.SetActive(true);
                        newNPC = npc.obj;
                    }
                    else
                    {
                        newNPC = Instantiate(Resources.Load<GameObject>(npc.path), transform);
                    }
                }
                else
                {
                    newNPC = npc.obj;
                }

                newNPC.GetComponent<NPC>().data = npc;
                newNPC.GetComponent<NPC>().ResetNPC();
                npc.obj = newNPC;

                if (npc.spawnPosition != Vector3.zero)
                    newNPC.transform.position = npc.spawnPosition;
                else
                    newNPC.transform.position = GetRandomSpawnPosition(coord);


                iterations++;
                if (iterations > maxInterationsPerFrame)
                {
                    yield return null;
                    iterations = 0;
                }
            }
        }
        yield return null;
    }

    private IEnumerator DespawnCoroutine(Vector2 coord)
    {
        int iterations = 0;
        int maxInterationsPerFrame = 1;
        foreach (AI npc in NPCs)
        {
            if (npc.coordinate == coord
                && npc.obj != null)
            {
                //Debug.Log("Resetting NPC");
                npc.spawnPosition = npc.obj.transform.position;
                npc.obj.GetComponent<NPC>().ResetNPC();
                if (destroyRatherThanDisable)
                {
                    Destroy(npc.obj);
                    npc.obj = null;
                }
                else
                {
                    npc.obj.SetActive(false);
                }
                iterations++;
                if (iterations > maxInterationsPerFrame)
                {
                    yield return null;
                    iterations = 0;
                }
            }
        }
        yield return null;
    }
}

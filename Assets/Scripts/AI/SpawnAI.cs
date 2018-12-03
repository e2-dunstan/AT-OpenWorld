using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class SpawnAI : MonoBehaviour
{
    private AIContainer aiContainer;
    private List<AI> NPCs;

    public GameObject friendly;
    public GameObject enemy;

    private void Awake()
    {
        NPCs = new List<AI>();

        string friendlyPath = GetFilePath(friendly);

        foreach (ReadCSV.CSVData dataItem in GetComponent<ReadCSV>().ReadFile())
        {
            if (dataItem.count == 0)
                continue;

            for (int i = 0; i < dataItem.count; i++)
            {
                AI friendlyAI = new AI();
                friendlyAI.SetVariables(friendlyPath, new Vector2(dataItem.x, dataItem.y), AI.AIType.FRIENDLY);
                NPCs.Add(friendlyAI);
            }
        }
    }

    public void SpawnAIAtTile(Vector2 coord)
    {
        foreach (AI npc in NPCs)
        {
            if (npc.coordinate == coord)
            {
                GameObject newNPC = Instantiate(Resources.Load<GameObject>(npc.path), transform);
                newNPC.GetComponent<AIController>().data = npc;
            }
        }
    }

    public void SaveObjects()
    {
        aiContainer = new AIContainer();
        aiContainer.ai = NPCs;
        aiContainer.Save("Assets/Resources/ai.xml");
    }
    private string GetFilePath(GameObject _obj)
    {
        string path = AssetDatabase.GetAssetPath(_obj);

        string resourcePath = "Assets/Resources/";
        if (path.Contains(resourcePath))
        {
            path = path.Replace(resourcePath, "");
        }

        string[] splitString = path.Split('.');
        path = splitString[0];

        return path;
    }

    //public void SpawnNewHumanoids(Vector2 coordinate, float tileSize, int count = 1)
    //{
    //    for(int i = 0; i < count; i++)
    //    {
    //        Vector3 spawnPosition = new Vector3(Random.Range(coordinate.x, coordinate.x + tileSize),
    //                                            0,
    //                                            Random.Range(coordinate.y, coordinate.y + tileSize));
    //        Spawn(humanoid, coordinate, spawnPosition);
    //    }
    //}

    //public void Spawn(GameObject newAI, Vector2 coordinate, Vector3 spawnPosition)
    //{
    //    NavMeshHit hit;
    //    if (NavMesh.SamplePosition(spawnPosition, out hit, 10, -1))
    //    {
    //        spawnPosition = hit.position;
    //    }

    //    GameObject instance = GetPooledObject();
    //    instance.GetComponent<AIController>().coordinate = coordinate;
    //    instance.transform.position = spawnPosition;
    //    instance.SetActive(true);
    //}

    //public void DestroyAI(Vector2 coordinate)
    //{
    //    foreach(GameObject npc in npcs)
    //    {
    //        if (npc.GetComponent<AIController>().coordinate == coordinate)
    //        {
    //            npc.SetActive(false);
    //        }
    //    }
    //}

    //public GameObject GetPooledObject()
    //{
    //    for (int i = 0; i < npcs.Count; i++)
    //    {
    //        if (!npcs[i].activeInHierarchy)
    //        {
    //            return npcs[i];
    //        }
    //    }
    //    return null;
    //}
}

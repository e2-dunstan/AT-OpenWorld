using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

/* To do:
 * save unloaded AI with positions etc when leaving tile so they return to the same positions
 */

public class SpawnAI : MonoBehaviour
{
    private AIContainer aiContainer;
    private List<AI> NPCs;

    public GameObject friendly;
    public GameObject enemy;

    public int friendliesPerEnemy = 5;

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
                if (i < dataItem.count / 5)
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

    public void SpawnAIAtTile(Vector2 coord)
    {
        foreach (AI npc in NPCs)
        {
            if (npc.coordinate == coord)
            {
                GameObject newNPC = Instantiate(Resources.Load<GameObject>(npc.path), transform);
                newNPC.GetComponent<NPC>().data = npc;
                npc.active = true;
            }
        }
    }

    //NOT IMPLEMENTED
    public void DespawnAIAtTile(Vector2 coord)
    {
        foreach (AI npc in NPCs)
        {
            if (npc.coordinate == coord)
            {
                npc.active = false;
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

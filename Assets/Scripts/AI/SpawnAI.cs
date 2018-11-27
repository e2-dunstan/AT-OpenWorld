using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnAI : MonoBehaviour
{
    public GameObject humanoid;

    //Using object pooling
    [HideInInspector]
    public List<GameObject> npcs;
    public int total = 100;

    private void Start()
    {
        npcs = new List<GameObject>();

        for (int i = 0; i < total; i++)
        {
            GameObject obj = Instantiate(humanoid, transform);
            obj.SetActive(false);
            npcs.Add(obj);
        }
    }

    public void SpawnNewHumanoids(Vector2 coordinate, float tileSize, int count = 1)
    {
        for(int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(coordinate.x, coordinate.x + tileSize),
                                                0,
                                                Random.Range(coordinate.y, coordinate.y + tileSize));
            Spawn(humanoid, coordinate, spawnPosition);
        }
    }

    public void Spawn(GameObject newAI, Vector2 coordinate, Vector3 spawnPosition)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(spawnPosition, out hit, 10, -1))
        {
            spawnPosition = hit.position;
        }

        GameObject instance = GetPooledObject();
        instance.GetComponent<AIController>().coordinate = coordinate;
        instance.transform.position = spawnPosition;
        instance.SetActive(true);
    }

    public void DestroyAI(Vector2 coordinate)
    {
        foreach(GameObject npc in npcs)
        {
            if (npc.GetComponent<AIController>().coordinate == coordinate)
            {
                npc.SetActive(false);
            }
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < npcs.Count; i++)
        {
            if (!npcs[i].activeInHierarchy)
            {
                return npcs[i];
            }
        }
        return null;
    }
}

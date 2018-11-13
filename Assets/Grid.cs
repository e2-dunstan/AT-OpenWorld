using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public bool renderGrid = true;

    public GameObject tilePrefab;
    public int tileSize = 1;
    public Vector2 gridDimensions;
    
    private List<Vector2> coordinates;

    private List<GameObject> terrainObjects;
    private List<GameObject> buildings;
    private List<GameObject> vegetation;

    private void Start()
    {
        for (int x = 0; x < gridDimensions.x; x++)
        {
            for (int y = 0; y < gridDimensions.y; y++)
            {
                Vector3 pos = new Vector3(transform.position.x + (x * tileSize), 
                                          10, 
                                          transform.position.z + (y * tileSize));
                GameObject newTile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                newTile.transform.localScale = new Vector3(tileSize / 10, 1, tileSize / 10);

                if (!renderGrid)
                {
                    newTile.GetComponent<MeshRenderer>().enabled = false;
                }

                coordinates.Add(new Vector2(pos.x, pos.z));
            }
        }

        GetObjectsInLayers();
    }

    private void GetObjectsInLayers()
    {
        //Layer 9 = terrain
        //Layer 10 = building
        //Layer 11 = vegetation

        GameObject[] allObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];

        foreach(GameObject obj in allObjects)
        {
            if (obj.layer == 9)
            {
                terrainObjects.Add(obj);
            }
            else if (obj.layer == 10)
            {
                buildings.Add(obj);
            }
            else if (obj.layer == 11)
            {
                vegetation.Add(obj);
            }
        }
    }







    //   struct Coordinates
    //   {
    //       public float x;
    //       public float z;
    //   }
    //   private Coordinates[] coordinates;

    //   public Vector2 origin;
    //   public Vector2 size;
    //   private Vector2 actualSize;

    //   public int gridSpacing = 100;

    //   private void OnDrawGizmosSelected()
    //   {
    //       Vector2 actualSize = origin + size;
    //       Gizmos.color = new Color(1, 0, 0, 1);

    //       //0,0
    //       Vector3 minXminZ = new Vector3(origin.x, 10, origin.y);
    //       //0,1
    //       Vector3 minXmaxZ = new Vector3(origin.x, 10, actualSize.y);
    //       //1,0
    //       Vector3 maxXminZ = new Vector3(actualSize.x, 10, origin.y);
    //       //1,1
    //       Vector3 maxXmaxZ = new Vector3(actualSize.x, 10, actualSize.y);

    //       Gizmos.DrawLine(minXminZ, minXmaxZ);
    //       Gizmos.DrawLine(minXminZ, maxXminZ);
    //       Gizmos.DrawLine(maxXminZ, maxXmaxZ);
    //       Gizmos.DrawLine(minXmaxZ, maxXmaxZ);

    //       for (int i = 0; i < (size.x / gridSpacing); i += gridSpacing)
    //       {
    //           Vector3 lineX0 = new Vector3(origin.x + gridSpacing * i, 10, origin.y);
    //           Vector3 lineX1 = new Vector3(origin.x + gridSpacing * i, 10, actualSize.y);
    //           Gizmos.DrawLine(lineX0, lineX1);
    //       }
    //       for (int i = 0; i < (actualSize.y / gridSpacing); i += gridSpacing)
    //       {
    //           Vector3 lineX0 = new Vector3(origin.x, 10, origin.y + gridSpacing * i);
    //           Vector3 lineX1 = new Vector3(actualSize.x, 10, origin.y + gridSpacing * i);
    //           Gizmos.DrawLine(lineX0, lineX1);
    //       }

    //   }

    //   void Start ()
    //   {
    //       actualSize = origin + size;

    //       GetAllObjectsInScene();
    //}

    //   void GetAllObjectsInScene()
    //   {

    //   }
}

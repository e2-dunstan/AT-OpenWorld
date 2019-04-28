using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridClass : MonoBehaviour
{
    public static GridClass grid;

    public Transform objectTransform;

    private ObjectContainer objectContainer;

    public bool renderGrid = true;

    public GameObject tilePrefab;
    public int tileSize = 1;
    public Vector2 gridDimensions;
    
    [HideInInspector]
    public List<Vector2> coordinates = new List<Vector2>();
    private List<SceneObject> objects = new List<SceneObject>();


    private void Awake()
    {
        // -- CREATE THE GRID -- //
        for (int x = 0; x < gridDimensions.x; x++)
        {
            for (int y = 0; y < gridDimensions.y; y++)
            {
                Vector3 pos = new Vector3(transform.position.x + (x * tileSize), 
                                          10, 
                                          transform.position.z + (y * tileSize));
                GameObject newTile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                newTile.transform.localScale = new Vector3(tileSize / 10, 1, tileSize / 10);
                newTile.name = newTile.transform.position.ToString();

                if (!renderGrid)
                {
                    newTile.GetComponent<MeshRenderer>().enabled = false;
                }
                coordinates.Add(new Vector2(pos.x, pos.z));
            }
        }

        GetObjectsInLayers();
        SaveObjects();
    }


    private void GetObjectsInLayers()
    {
        //Layer 9 = terrain
        //Layer 10 = building
        //Layer 11 = vegetation

        GameObject[] allObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];

        // -- FIND ALL OBJECTS IN THE SCENE -- //
        foreach(GameObject obj in allObjects)
        {
            //Only be concerned with these layers, discard all other objects
            if (obj.layer == 9 || obj.layer == 10 || obj.layer == 11)
            {
                Vector2 objPos = GetCoordinatesOfObject(obj);
                
                string path = GetFilePath(obj);

                if (objPos == Vector2.zero || path == "")
                {
                    //Debug.Log("Object does not lie in the grid: " + obj.name);
                }
                else
                {
                    Transform objTrans = obj.transform;
                    SceneObject newObject = new SceneObject();
                    //newObject.SetVariables(obj.name, path, 
                    //    objTrans.position, objTrans.eulerAngles, objTrans.localScale,
                    //    objPos, (SceneObject.Type)obj.layer);
                    objects.Add(newObject);

                    Destroy(obj);
                }
            }
        }
    }

    private string GetFilePath(GameObject _obj)
    {
        Object parentObject = PrefabUtility.GetCorrespondingObjectFromSource(_obj);
        string path = AssetDatabase.GetAssetPath(parentObject);

        string resourcePath = "Assets/Resources/";
        if (path.Contains(resourcePath))
        {
            path = path.Replace(resourcePath, "");
        }

        string[] splitString = path.Split('.');
        path = splitString[0];

        return path;
    }

    public Vector2 GetCoordinatesOfObject(GameObject obj)
    {
        foreach (Vector2 coord in coordinates)
        {
            //Object is in this tile
            if (obj.transform.position.x >= coord.x 
                && obj.transform.position.z >= coord.y
                && obj.transform.position.x < (coord.x + tileSize) 
                && obj.transform.position.z < (coord.y + tileSize))
            {
                return coord;
            }
        }
        return Vector2.zero;
    }

    public void SaveObjects()
    {
        objectContainer = new ObjectContainer();
        //objectContainer.sceneObjects = objects;
        //objectContainer.Save("Assets/Resources/sceneobjects.xml");
    }

    public IEnumerator ToggleObjectsAtTile(Vector2 coord, bool enable)
    {
        //ObjectContainer objContainer = ObjectContainer.Load("Assets/Resources/sceneobjects.xml");

        //foreach(SceneObject obj in objContainer.sceneObjects)
        //{
            //if (obj.coordinate == coord && enable)
            //{
            //    GameObject newObject = Instantiate(Resources.Load<GameObject>(obj.path), objectTransform);
            //    newObject.transform.position = obj.position;
            //    newObject.transform.eulerAngles = obj.rotation;
            //    newObject.transform.localScale = obj.scale;
            //    newObject.layer = (int)obj.type;
            //    //obj.gameObject.SetActive(enable);
            //}

            yield return null;
        //}
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

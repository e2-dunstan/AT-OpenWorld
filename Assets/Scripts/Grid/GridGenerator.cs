using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridGenerator : MonoBehaviour
{
    public Transform objectTransform;

    private ObjectContainer objectContainer;
    
    public GameObject tilePrefab;
    public int tileSize = 1;
    public Vector2 gridDimensions;

    [HideInInspector]
    public List<Tile> tiles = new List<Tile>();
    private List<GameObject> allObjectsInScene = new List<GameObject>();
    private List<SceneObject> sceneObjects = new List<SceneObject>();

    //Grid is only visible in editor
    public void Start()
    {
        Generate();

        foreach(Tile tile in tiles)
        {
            tile.tileObject.GetComponent<MeshRenderer>().enabled = false;
        }
        foreach (GameObject gameObject in allObjectsInScene)
        {
            Destroy(gameObject);
        }
    }

    public void Generate()
    {
        //Clear lists
        //tiles.Clear();
        //allObjectsInScene.Clear();
        //sceneObjects.Clear();

        // -- Get all objects which are children of the object transform -- //
        Transform[] allChildren = objectTransform.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child != objectTransform)
                allObjectsInScene.Add(child.gameObject);
        }

        // -- Generate Grid -- //
        for (int x = 0; x < gridDimensions.x; x++)
        {
            for (int y = 0; y < gridDimensions.y; y++)
            {
                Vector3 pos = new Vector3(transform.position.x + (x * tileSize),
                                          10,
                                          transform.position.z + (y * tileSize));
                GameObject newTile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                newTile.transform.localScale = new Vector3(tileSize / 10, 1, tileSize / 10);
                //Set the name in the inspector for debug purposes
                newTile.name = newTile.transform.position.ToString();

                Tile tile = new Tile();
                tile.tileObject = newTile;
                tile.coordinate = new Vector2(x, y);
                tile.worldPosition = pos;

                //Find all objects in this tile
                GetObjectsInTile(tile, newTile);

                tiles.Add(tile);
            }
        }
        SaveObjects();
    }

    private void GetObjectsInTile(Tile t, GameObject newParent)
    {
        //Layer 9 = terrain
        //Layer 10 = building
        //Layer 11 = vegetation

        List<GameObject> temp = allObjectsInScene;

        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i].layer == 9 || temp[i].layer == 10 || temp[i].layer == 11)
            {
                //If the object is in this tile
                if (temp[i].transform.position.x >= t.worldPosition.x
                    && temp[i].transform.position.z >= t.worldPosition.z
                    && temp[i].transform.position.x < t.worldPosition.x + tileSize
                    && temp[i].transform.position.z < t.worldPosition.z + tileSize)
                {
                    //Get the file path from the resources folder
                    string path = GetFilePath(temp[i]);
                    
                    SceneObject newObject = new SceneObject();
                    newObject.SetVariables(temp[i].name, path,
                        temp[i].transform.position, temp[i].transform.eulerAngles, temp[i].transform.localScale,
                        t.worldPosition, t.coordinate, (SceneObject.Type)temp[i].layer);

                    sceneObjects.Add(newObject);

                    if (temp[i].layer == 9)
                        t.terrain.Add(newObject);
                    else if (temp[i].layer == 10)
                        t.buildings.Add(newObject);
                    else if (temp[i].layer == 11)
                        t.vegetation.Add(newObject);

                    temp[i].transform.parent = newParent.transform;
                    //Don't loop with this object anymore
                    //Debug.Log("Removing item: " + temp[i].name);
                    //allObjectsInScene.Remove(temp[i]);
                }
            }
            //Discard object if it's not on any of the target layers
            else
            {
                //Debug.Log("Removing item: " + temp[i].name);
                allObjectsInScene.Remove(temp[i]);
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

    public void Clear()
    {
        foreach(Transform tile in transform)
        {
            if (tile != transform)
            {
                DestroyImmediate(tile.gameObject);
            }
        }
    }

    public void SaveObjects()
    {
        objectContainer = new ObjectContainer();
        objectContainer.sceneObjects = sceneObjects;
        objectContainer.Save("Assets/Resources/sceneobjects.xml");
    }

    public IEnumerator ToggleObjectsAtTile(Vector2 coord, bool enable)
    {
        ObjectContainer objContainer = ObjectContainer.Load("Assets/Resources/sceneobjects.xml");
        Tile tile = new Tile();

        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].coordinate == coord)
            {
                tile = tiles[i];
            }
        }

        if (enable && !tile.loaded)
        {
            tile.loaded = true;
            foreach (SceneObject obj in objContainer.sceneObjects)
            {
                if (obj.coordinate == coord)
                {
                    if (Resources.Load<GameObject>(obj.path) != null)
                    {
                        GameObject newObject = Instantiate(Resources.Load<GameObject>(obj.path), objectTransform);
                        newObject.transform.position = obj.position;
                        newObject.transform.eulerAngles = obj.rotation;
                        newObject.transform.localScale = obj.scale;
                        newObject.layer = (int)obj.type;
                        
                        tile.objects.Add(newObject);
                    }
                    yield return null;
                }
            }
        }
        else if (!enable)
        {
            tile.loaded = false;
            foreach (GameObject obj in tile.objects)
            {
                Destroy(obj);
            }
        }
    }
}

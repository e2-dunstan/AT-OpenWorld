using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public bool loaded = false;

    public int id;

    public GameObject tileObject;
    public Vector2 coordinate;
    public Vector3 worldPosition;

    public List<GameObject> objects = new List<GameObject>(); 

    public List<SceneObject> buildings = new List<SceneObject>();
    public List<SceneObject> terrain = new List<SceneObject>();
    public List<SceneObject> vegetation = new List<SceneObject>();
    public List<SceneObject> effects = new List<SceneObject>();
}

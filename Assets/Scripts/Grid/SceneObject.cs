using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class SceneObject
{
    public enum Type
    {
        terrain = 9,
        building = 10,
        vegetation = 11
    }

    public GameObject obj;
    public Transform transform;
    public Vector2 coordinate;
    public Type type;
    
    public SceneObject(GameObject _obj, Vector2 _coordinate, Type _type)
    {
        obj = _obj;
        coordinate = _coordinate;
        type = _type;
    }
}

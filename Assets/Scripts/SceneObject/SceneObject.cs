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

    public string name;
    public string path;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    public Vector2 coordinate;
    public Type type;
    
    public void SetVariables(string _name, string _path, 
        Vector3 _position, Vector3 _rotation, Vector3 _scale, 
        Vector2 _coordinate, Type _type)
    {
        name = _name;
        path = _path;
        position = _position;
        rotation = _rotation;
        scale = _scale;
        coordinate = _coordinate;
        type = _type;
    }
}

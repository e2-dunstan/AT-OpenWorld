using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI
{
    public enum AIType
    {
        FRIENDLY, ENEMY
    }
    public AIType type;
    public string path;
    public Vector2 coordinate;

    public void SetVariables(string _path, Vector2 _coordinate, AIType _type)
    {
        path = _path;
        coordinate = _coordinate;
        type = _type;
    }
}

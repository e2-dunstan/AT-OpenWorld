﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPosition;
    public Vector2 gridPosition;

    //Distance from starting node
    public int gCost;
    //Distance from end node
    public int hCost;

    public Node parentNode;
    private int heapIndex;

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public Node(bool _walkable, Vector3 _worldPosition, Vector2 _gridPosition)
    {
        walkable = _walkable;
        worldPosition = _worldPosition;
        gridPosition = _gridPosition;
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}

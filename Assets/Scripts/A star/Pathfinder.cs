using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private PathRequestManager requestManager;

    private void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
    }

    public void StartFindPath(Vector3 _startPosition, Vector3 _targetPosition)
    {
        StartCoroutine(FindPath(_startPosition, _targetPosition));
    }

    private IEnumerator FindPath(Vector3 _startPosition, Vector3 _targetPosition)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = AStarGrid.g.GetNodeFromWorldPosition(_startPosition);
        Node targetNode = AStarGrid.g.GetNodeFromWorldPosition(_targetPosition);

        //if the nodes are not obstacles
        if (startNode.walkable && targetNode.walkable)
        {
            //Nodes to evaluate
            Heap<Node> openList = new Heap<Node>((int)(AStarGrid.g.gridSize.x * AStarGrid.g.gridSize.y));
            //Already evaluated nodes
            List<Node> closedList = new List<Node>();

            startNode.gCost = 0;
            openList.Add(startNode);

            //Loop through open list
            while (openList.Count > 0)
            {
                //Heap optimisation - WRITE ABOUT THIS IN THE REPORT
                //Remove current node from the open list because it is being evaluated
                Node currentNode = openList.RemoveFirst();

                // -- Very slow -- //
                /*Node lowestFCostNode = null;
                foreach(Node n in openList)
                {
                    if ((lowestFCostNode != null && lowestFCostNode.fCost > n.fCost)
                        || (n.fCost == lowestFCostNode.fCost && n.hCost < lowestFCostNode.hCost))
                        lowestFCostNode = n;
                }
                Node currentNode = lowestFCostNode;*/

                //Add the current node to the closed (evaluated) list
                closedList.Add(currentNode);

                //Path found
                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbour in AStarGrid.g.GetNodeNeighbours(currentNode))
                {
                    //if the neighbour is an obstacle or has already been evaluated
                    //or is in a building
                    if (!neighbour.walkable || closedList.Contains(neighbour))
                    {
                        continue;
                    }
                    
                    int movementCost = currentNode.gCost + GetDistanceBetweenNodes(currentNode, neighbour);
                    if (movementCost < neighbour.gCost || !openList.Contains(neighbour))
                    {
                        //set f cost of neighbour (g cost + h cost)
                        neighbour.gCost = movementCost;
                        neighbour.hCost = GetDistanceBetweenNodes(neighbour, targetNode);
                        //set parent of neighbour to current
                        neighbour.parentNode = currentNode;

                        if (!openList.Contains(neighbour))
                            openList.Add(neighbour);
                        else
                            openList.UpdateItem(neighbour);
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    private Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }
        path.Add(startNode);
        
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);

        //path smoothing here

        return waypoints;
    }

    private Vector3[] SimplifyPath(List<Node> _path)
    {
        List<Vector3> waypoints = new List<Vector3>();

        Vector2 oldDirection = Vector2.zero;

        for (int i = 1; i < _path.Count; i++)
        {
            //direction between two nodes
            Vector2 newDirection = new Vector2(_path[i - 1].gridPosition.x - _path[i].gridPosition.x,
                                               _path[i - 1].gridPosition.y - _path[i].gridPosition.y);
            //if the direction has changed i.e. a corner has been found
            if (newDirection != oldDirection)
            {
                //only keep the corner
                waypoints.Add(_path[i - 1].worldPosition);
            }
            oldDirection = newDirection;
        }
        return waypoints.ToArray();
    }
    private int GetDistanceBetweenNodes(Node nodeA, Node nodeB)
    {
        Vector2Int distance = new Vector2Int(
            (int)Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x),
            (int)Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y));

        //14 for diagonal, 10 for horizontal/vertical
        if (distance.x > distance.y)
            return (14 * distance.y) + (10 * (distance.x - distance.y));
        else
            return (14 * distance.x) + (10 * (distance.y - distance.x));
    }
}

using System.Collections;
using System.Collections.Generic;
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

        Node startNode = Grid.g.GetNodeFromWorldPosition(_startPosition);
        Node targetNode = Grid.g.GetNodeFromWorldPosition(_targetPosition);

        if (startNode.walkable && targetNode.walkable)
        {
            //Nodes to evaluate
            Heap<Node> openList = new Heap<Node>((int)(Grid.g.gridSize.x * Grid.g.gridSize.y));
            //Already evaluated nodes
            List<Node> closedList = new List<Node>();

            startNode.gCost = 0;
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                //Heap optimisation - WRITE ABOUT THIS IN THE REPORT
                Node currentNode = openList.RemoveFirst();
                //Very slow
                //Node lowestFCostNode = null;
                //foreach(Node n in openList)
                //{
                //    if ((lowestFCostNode != null && lowestFCostNode.fCost > n.fCost)
                //        || (n.fCost == lowestFCostNode.fCost && n.hCost < lowestFCostNode.hCost))
                //        lowestFCostNode = n;
                //}
                //Node currentNode = lowestFCostNode;

                closedList.Add(currentNode);

                //Path found
                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbour in Grid.g.GetNodeNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedList.Contains(neighbour))
                        continue;

                    int movementCost = currentNode.gCost + GetDistanceBetweenNodes(currentNode, neighbour);
                    if (movementCost < neighbour.gCost || !openList.Contains(neighbour))
                    {
                        //set f cost of neighbour
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
        path.Reverse();
        return path;
    }

    private int GetDistanceBetweenNodes(Node nodeA, Node nodeB)
    {
        Vector2 distance = new Vector2(
            Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.y),
            Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.x));

        //14 for diagonal, 10 for horizontal/vertical
        if (distance.x > distance.y)
            return (int)(14 * distance.y + 10 * (distance.x - distance.y));
        else
            return (int)(14 * distance.x + 10 * (distance.y - distance.x));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarGrid : MonoBehaviour
{
    public static AStarGrid g;
    public bool showGrid = false;

    public GridGenerator streamingGrid;

    //buildings and vegetation
    public LayerMask unwalkableLayers;

    public Vector2 gridDimensions;
    public float nodeRadius;
    private float nodeDiameter;

    [HideInInspector]
    public Node[,] grid;
    [HideInInspector]
    public Node targetNode;
    [HideInInspector]
    public Vector2 gridSize;

    private void Awake()
    {
        if (g == null)
            g = this;
        
        nodeDiameter = nodeRadius * 2;
        gridSize.x = Mathf.RoundToInt(gridDimensions.x / nodeDiameter);
        gridSize.y = Mathf.RoundToInt(gridDimensions.y / nodeDiameter);

        GenerateGrid();
    }

    private void GenerateGrid()
    {
        grid = new Node[(int)gridSize.x, (int)gridSize.y];
        //Bottom left
        Vector3 worldZeroCoord = transform.position
            - ((Vector3.right * gridDimensions.x) / 2)
            - ((Vector3.forward * gridDimensions.y) / 2);

        //Initialise nodes
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 worldPoint = worldZeroCoord
                    + Vector3.right * (x * nodeDiameter + nodeRadius)
                    + Vector3.forward * (y * nodeDiameter + nodeRadius);


                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableLayers);
                if (Physics.OverlapSphere(worldPoint, 0, unwalkableLayers).Length > 0)
                {
                    walkable = false;
                }
                //Does not work properly for non-primitive colliders
                //Collider[] collidersInRange = Physics.OverlapSphere(worldPoint, 8, unwalkableLayers);
                //bool walkable = true;
                //if (collidersInRange.Length > 0)
                //{
                //    for (int i = 0; i < collidersInRange.Length; i++)
                //    {
                //        if (collidersInRange[i].bounds.Contains(worldPoint))
                //        {
                //            walkable = false;
                //        }
                //    }
                //}
                grid[x, y] = new Node(walkable, worldPoint, new Vector2(x, y));
            }
        }
        grid[0, 0].walkable = false;
    }

    //Convert world position into grid position
    public Node GetNodeFromWorldPosition(Vector3 _worldPosition)
    {
        Vector2 approx = new Vector2(
            Mathf.Clamp01((_worldPosition.x + gridDimensions.x / 2) / gridDimensions.x),
            Mathf.Clamp01((_worldPosition.z + gridDimensions.y / 2) / gridDimensions.y));

        return grid[Mathf.RoundToInt((gridSize.x - 1) * approx.x),
                    Mathf.RoundToInt((gridSize.y - 1) * approx.y)];
    }

    public List<Node> GetNodeNeighbours(Node _node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                //If this is the node being compared, skip
                if (x == 0 && y == 0)
                    continue;

                Vector2 newPos = new Vector2(_node.gridPosition.x + x, _node.gridPosition.y + y);
                
                //if this position is in the grid, it is valid
                if (newPos.x >= 0 && newPos.x < gridSize.x
                    && newPos.y >= 0 && newPos.y < gridSize.y)
                {
                    Ray ray = new Ray(_node.worldPosition, grid[(int)newPos.x, (int)newPos.y].worldPosition - _node.worldPosition);
                    float distance = Vector3.Distance(_node.worldPosition, grid[(int)newPos.x, (int)newPos.y].worldPosition);

                    if (!Physics.Raycast(ray, distance, unwalkableLayers))
                    {
                        neighbours.Add(grid[(int)newPos.x, (int)newPos.y]);
                    }
                    else
                    {
                        //Debug.LogWarning("Node in building found");
                        grid[(int)newPos.x, (int)newPos.y].walkable = false;
                    }
                }
            }
        }
        return neighbours;
    }

    //Debug
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridDimensions.x, 1, gridDimensions.y));

        if (grid != null && showGrid)
        {
            foreach (Node n in grid)
            {
                if (n == targetNode)
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = (n.walkable) ? Color.white : Color.red;
                }
                Gizmos.DrawCube(n.worldPosition, Vector3.one * nodeDiameter * 0.3f);
            }
        }
    }
}

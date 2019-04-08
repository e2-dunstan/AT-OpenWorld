using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    //DESPAWN IF IT LEAVES LOADED TILES?
    //NEVER ROAM OUTSIDE OF ITS TILE?   <-- implemented

    //ISSUE ENCOUNTERED: NODES INSIDE OF BUILDINGS ARE TAGGED AS WALKABLE


    [HideInInspector]
    public AI data;
    private GridGenerator grid;
    private Animator anim;
    
    // -- Movement/Transform variables -- //
    public float animationSpeed = 10.0f;
    public float movementSpeed = 5.0f;
    private Vector3 previousPosition;
    
    private Vector3 target;
    private Vector3 currentTarget;
    
    // -- A* variables -- //
    private Vector3[] path;
    private int targetWaypoint;
    private bool targetReached = false;

    public GameObject debugTarget;

    private void Start()
    {
        grid = GetComponentInParent<ReadCSV>().gridGenerator;
        anim = GetComponent<Animator>();

        previousPosition = transform.position;

        //StartCoroutine(SetNewTarget());
        SetNewTarget();
    }

    private void Update()
    {
        if (target != currentTarget)
        {
            Debug.Log("Requesting new path");
            PathRequestManager.RequestPath(transform.position, target, OnPathFound);
            currentTarget = target;
        }

        float speed = Vector3.Distance(previousPosition, transform.position) / Time.deltaTime;
        previousPosition = transform.position;
        anim.SetFloat("Speed", speed / animationSpeed);
    }
    
    //////////////
    // -- A* -- //
    //////////////
    private void SetNewTarget()
    {
        Node randomNode = AStarGrid.g.grid[0, 0];

        //if (currentTarget == null)
        //    currentTarget = randomNode.worldPosition;

        while (randomNode.locationInStreamingGrid != data.coordinate
            || randomNode.worldPosition == currentTarget
            || !randomNode.walkable)
        {
            Debug.Log("Finding new node");
            randomNode = AStarGrid.g.grid[
            (int)Random.Range(0, AStarGrid.g.gridSize.x - 1),
            (int)Random.Range(0, AStarGrid.g.gridSize.y - 1)];
            //yield return null;
        }
        Debug.Log("New target node: " + randomNode.gridPosition + " Tile: " + randomNode.locationInStreamingGrid);

        //Node randomNode = AStarGrid.g.grid[
        //    (int)Random.Range(0, AStarGrid.g.gridSize.x - 1),
        //    (int)Random.Range(0, AStarGrid.g.gridSize.y - 1)];

        target = randomNode.worldPosition;
        //Instantiate(debugTarget, randomNode.worldPosition, Quaternion.identity);
    }

    public void OnPathFound(Vector3[] _path, bool _pathSuccess)
    {
        if (_pathSuccess)
        {
            path = _path;
            StopCoroutine(FollowPath());
            StartCoroutine(FollowPath());
            currentTarget = target;
        }
        else
        {
            SetNewTarget();
        }
    }

    private IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            //rotation speed relative to the movement speed
            if (Vector3.Distance(transform.position, currentWaypoint) < 1)
            {
                targetWaypoint++;
                if (targetWaypoint >= path.Length)
                {
                    //exit coroutine and find new target
                    //StartCoroutine(SetNewTarget());
                    Debug.Log("Destination reached");
                    SetNewTarget();
                    yield break;
                }
                currentWaypoint = path[targetWaypoint];
            }
            
            Vector3 direction = (currentWaypoint - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                                            transform.rotation, lookRotation,
                                            movementSpeed * Time.deltaTime);
            transform.position += transform.forward * Time.deltaTime * movementSpeed;

            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for(int i = targetWaypoint; i < path.Length; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(path[i], Vector3.one * 0.5f);

                if (i == targetWaypoint)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    //DESPAWN IF IT LEAVES LOADED TILES?
    //NEVER ROAM OUTSIDE OF ITS TILE?   <-- implemented


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

    private void Start()
    {
        grid = GetComponentInParent<ReadCSV>().gridGenerator;
        anim = GetComponent<Animator>();

        previousPosition = transform.position;

        StartCoroutine(SetNewTarget());
    }

    private void Update()
    {
        if (target != currentTarget)
        {
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
    private IEnumerator SetNewTarget()
    {
        Node randomNode;
        do
        {
            randomNode = AStarGrid.g.grid[
            (int)Random.Range(0, AStarGrid.g.gridSize.x),
            (int)Random.Range(0, AStarGrid.g.gridSize.y)];
            yield return null;
        }
        while (randomNode.locationInStreamingGrid != data.coordinate);

        Debug.Log("target found. Node: " + randomNode.locationInStreamingGrid + " Tile: " + data.coordinate);
        target = randomNode.worldPosition;
    }

    public void OnPathFound(Vector3[] _path, bool _pathSuccess)
    {
        if (_pathSuccess)
        {
            path = _path;
            StopCoroutine(FollowPath());
            StartCoroutine(FollowPath());
        }
    }

    IEnumerator FollowPath()
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
                    StartCoroutine(SetNewTarget());
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


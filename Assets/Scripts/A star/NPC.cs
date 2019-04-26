using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    //DESPAWN IF IT LEAVES LOADED TILES?
    //NEVER ROAM OUTSIDE OF ITS TILE?   <-- implemented

    //ISSUE ENCOUNTERED: NODES INSIDE OF BUILDINGS ARE TAGGED AS WALKABLE
    public enum State { IDLE, ROAMING, CHASING, ATTACKING }
    [HideInInspector]
    public State state = State.IDLE;

    //private int roamRange = 10;

    private EnemyController enemy;

    [HideInInspector]
    public AI data;
    //private GridGenerator grid;
    [HideInInspector]
    public Animator anim;
    
    // -- Movement/Transform variables -- //
    [HideInInspector]
    public float animationSpeed = 10.0f;
    //Must be set in the inspector
    public float movementSpeed = 2.5f;
    private Vector3 previousPosition;

    #region A* variables
    // -- A* variables -- //
    private Vector3 target;
    private Vector3 currentTarget;
    private Vector3[] path;
    private int targetWaypoint;
    //private bool targetReached = false;
    #endregion

    //public GameObject debugTarget;

    private void Start()
    {
        //grid = GetComponentInParent<ReadCSV>().gridGenerator;
        anim = gameObject.GetComponent<Animator>();

        if (data.type == AI.AIType.ENEMY)
        {
            enemy = gameObject.AddComponent<EnemyController>();
            enemy.npc = this;
        }

        if (data.spawnPosition != Vector3.zero)
        {
            transform.position = data.spawnPosition;
        }
        else
        {
            transform.position = GetComponentInParent<SpawnAI>().GetRandomSpawnPosition(data.coordinate);
        }

        previousPosition = transform.position;
        SetNewTarget();
    }

    private void Update()
    {
        Vector2 currentPlayerTile = new Vector2(PlayerGrid.g.currentTile.coordinate.x + 1,
                                                PlayerGrid.g.currentTile.coordinate.y + 1);
        switch (state)
        {
            case State.IDLE:
                if (path != null)
                    path = null;
                if (data.coordinate == currentPlayerTile)
                    state = State.ROAMING;
                break;

            case State.ROAMING:
                if (data.coordinate != currentPlayerTile)
                {
                    state = State.IDLE;
                    StopCoroutine(FollowPath());
                }
                break;

            case State.CHASING:
                StopCoroutine(FollowPath());
                if (path != null)
                    path = null;
                enemy.Chasing();
                break;

            case State.ATTACKING:
                StopCoroutine(FollowPath());
                if (path != null)
                    path = null;
                enemy.Attacking();
                break;
        }
        
        if (target != currentTarget && data.coordinate == currentPlayerTile)
        {
            //Debug.Log("Requesting new path");
            PathRequestManager.RequestPath(transform.position, target, OnPathFound);
            currentTarget = target;
        }

        //Speed of the animation is determined by the speed of the movement
        float speed = Vector3.Distance(previousPosition, transform.position) / Time.deltaTime;
        previousPosition = transform.position;
        anim.SetFloat("Speed", speed / animationSpeed);
        //FollowPath coroutine controls actual movement
    }

    #region A* region
    //////////////
    // -- A* -- //
    //////////////
    public void SetNewTarget()
    {
        Node randomNode = AStarGrid.g.grid[0, 0];

        while ((randomNode.locationInStreamingGrid != data.coordinate
            || randomNode.worldPosition == currentTarget || !randomNode.walkable)
            && state != State.ATTACKING && state != State.CHASING)
        {
            randomNode = AStarGrid.g.grid[
            (int)Random.Range(0, AStarGrid.g.gridSize.x - 1),
            (int)Random.Range(0, AStarGrid.g.gridSize.y - 1)];
        }
        //Debug.Log("New target node: " + randomNode.gridPosition + " Tile: " + randomNode.locationInStreamingGrid);
        target = randomNode.worldPosition;
        //Instantiate(debugTarget, randomNode.worldPosition, Quaternion.identity);
    }

    public void OnPathFound(Vector3[] _path, bool _pathSuccess)
    {
        if (state != State.CHASING && state != State.ATTACKING)
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
    }
    #endregion

    private IEnumerator FollowPath()
    {
        if (path.Length <= 0)
        {
            yield break;
        }
        Vector3 currentWaypoint = path[0];

        while (state == State.ROAMING)
        {
            //rotation speed relative to the movement speed
            if (Vector3.Distance(transform.position, currentWaypoint) < 1)
            {
                targetWaypoint++;
                if (targetWaypoint >= path.Length) //destination reached
                {
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

    public void ResetNPC()
    {
        StopAllCoroutines();
        //PathRequestManager.prm.ClearQueue();
        PathRequestManager.prm.pathfinder.StopAllCoroutines();

        if (transform.position != Vector3.zero)
            data.spawnPosition = transform.position;

        path = null;
        //targetWaypoint = 0;
        //target = AStarGrid.g.grid[0, 0].worldPosition;
        //currentTarget = AStarGrid.g.grid[0, 0].worldPosition;
        SetNewTarget();

        if (anim != null && anim.GetBool("Running"))
            anim.SetBool("Running", false);
    }

    #region Debug
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
    #endregion
}
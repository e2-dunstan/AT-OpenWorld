using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [HideInInspector]
    public AI data;

    private GridGenerator grid;
    private NavMeshAgent agent;
    private Animator anim;
    private PlayerGrid player;

    private float range = 25.0f;


    private Vector3 targetPos;

    private bool moving = false;

    private void Start()
    {
        grid = GetComponentInParent<ReadCSV>().gridGenerator;

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerGrid>();

        transform.position = GetRandomPositionInTile();
    }

    private Vector3 GetRandomPositionInTile()
    {
        Vector2 coord = data.coordinate;
        Vector3 rand = grid.transform.position + 
                        new Vector3(Random.Range(coord.x * grid.tileSize, (coord.x * grid.tileSize) + grid.tileSize),
                                    0,
                                    Random.Range(coord.y * grid.tileSize, (coord.y * grid.tileSize) + grid.tileSize));
        Debug.Log("rand: " + rand);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(rand, out hit, range, -1))
            return hit.position;

        else return Vector3.zero;
    }

    private void Update()
    {
        anim.SetFloat("Speed", agent.velocity.magnitude / 5);

        if (!moving && player.currentTile.coordinate.x == data.coordinate.x
            && player.currentTile.coordinate.y == data.coordinate.y)
        {
            StartCoroutine(Move(0.1f));
            moving = true;
        }
        if (moving && player.currentTile.coordinate.x != data.coordinate.x
            && player.currentTile.coordinate.y != data.coordinate.y)
        {
            StopAllCoroutines();
            moving = false;
        }
    }

    IEnumerator Move(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        agent.destination = GetRandomPositionInTile();
        StartCoroutine(Move(Random.Range(3, 10)));
    }
}

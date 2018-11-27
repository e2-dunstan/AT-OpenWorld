using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private PlayerGrid player;

    private Vector3 targetPos;
    private float range = 25.0f;

    [HideInInspector]
    public Vector2 coordinate;

    private bool moving = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerGrid>();
    }

    private void Update()
    {
        anim.SetFloat("Speed", agent.velocity.magnitude / 5);

        Debug.Log("Player tile: " + player.currentTile.worldPosition);
        Debug.Log("AI tile: " + coordinate);

        if (!moving && player.currentTile.worldPosition.x == coordinate.x
            && player.currentTile.worldPosition.z == coordinate.y)
        {
            StartCoroutine(Move(0.1f));
            moving = true;
        }
        if (moving && player.currentTile.worldPosition.x != coordinate.x
            && player.currentTile.worldPosition.z != coordinate.y)
        {
            StopAllCoroutines();
            moving = false;
        }
    }

    IEnumerator Move(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Vector3 randomDirection = Random.insideUnitSphere * range;
        randomDirection += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, range, -1);
        targetPos = hit.position;
        agent.destination = targetPos;

        StartCoroutine(Move(Random.Range(3, 10)));
	}
}

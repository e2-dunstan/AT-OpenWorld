using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;

    private Vector3 targetPos;
    private float range = 25.0f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        StartCoroutine(Move(0.1f));
    }

    private void Update()
    {
        //if (Vector3.Distance(targetPos, transform.position) < 5.0f)
        //{
        //    float time = Random.Range(1, 5);
        //    StopAllCoroutines();
        //    StartCoroutine(Move(time));
        //}
        anim.SetFloat("Speed", agent.velocity.magnitude);
    }

    IEnumerator Move(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Vector3 randomDirection = Random.insideUnitSphere * range;
        randomDirection += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, range, -1);
        targetPos = hit.position;
        //targetPos = new Vector2(Random.Range(transform.position.x - range, transform.position.x + range),
        //                        Random.Range(transform.position.z - range, transform.position.z + range));
        agent.destination = targetPos;
        Debug.Log("New goal: " + targetPos);

        StartCoroutine(Move(Random.Range(3, 10)));
	}
}

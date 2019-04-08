using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KittenController : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent agent;
    private Animator anim;

    [HideInInspector]
    public bool following = false;

    private Vector3 playerPosition;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Destroy(transform.parent.GetComponentInChildren<ParticleSystem>());
            playerPosition = player.transform.position;
            following = true;
        }
    }

    private void Update()
    {
        if (!following)
            return;
        
        if (Vector3.Distance(playerPosition, player.transform.position) > 2)
        {
            playerPosition = player.transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(playerPosition, out hit, 3, -1))
                agent.destination = hit.position;
        }
        if (Vector3.Distance(playerPosition, transform.position) < 2)
        {
            agent.ResetPath();
        }
        anim.SetFloat("speed", agent.velocity.magnitude / 6);
    }
}

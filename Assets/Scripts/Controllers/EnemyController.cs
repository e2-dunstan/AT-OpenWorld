using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [HideInInspector]
    public NPC npc;
    private GameObject player;

    private float fov = 50;

    private float searchRange;
    //private bool hasRolled = false;
    private float chaseSpeed = 5;
    //private bool chasing = false;
    //public bool updatingPlayerPosition = false;

    private float attackRange = 2.0f;
    private float timeBetweenAttacks = 1.0f;
    private float timeElapsed = 1.0f;

    //private Vector3 lastPlayerLoc = Vector3.zero;
    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        searchRange = GetComponent<SphereCollider>().radius;
    }

    public void Chasing()
    {
        if (GetDistanceFromPlayer() > searchRange * 1.2f)
        {
            //chasing = false;
            npc.anim.SetBool("Running", false);
            
            npc.SetNewTarget();
            npc.state = NPC.State.IDLE;
        }
        else if (GetDistanceFromPlayer() < attackRange)
        {
            //chasing = false;
            npc.anim.SetBool("Running", false);
            npc.state = NPC.State.ATTACKING;
        }
        else
        {
            if (!npc.anim.GetBool("Running"))
                npc.anim.SetBool("Running", true);
            
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                                            transform.rotation, lookRotation,
                                            chaseSpeed * Time.deltaTime);
            transform.position += transform.forward * Time.deltaTime * chaseSpeed;
        }
    }
    //Using A* each frame to calculate the path to the player is too slow

    public void Attacking()
    {
        if (timeElapsed >= timeBetweenAttacks)
        {
            npc.anim.SetTrigger("Attack");
            GetComponent<AudioSource>().Play();
            timeElapsed = 0;
        }
        else
        {
            timeElapsed += Time.deltaTime;
        }

        if (GetDistanceFromPlayer() > attackRange * 1.2f)
        {
            npc.state = NPC.State.CHASING;
        }
    }

    //Called from animation event
    public void Hit()
    {
        if (GetDistanceFromPlayer() < 2.0f)
        {
            player.GetComponent<CatController>().TakeDamage();
        }
    }

    //private void SetLastLocationPlayerSpotted()
    //{
    //    Vector3 origin = transform.position + new Vector3(0, 1.5f, 0);
    //    RaycastHit hit;

    //    if (Physics.SphereCast(origin, searchRange, transform.forward, out hit))
    //    {
    //        if (hit.collider.tag == "Player")
    //        {
    //            lastPlayerLoc = player.transform.position;
    //        }
    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Vector3.Angle(transform.forward, 
                player.transform.position - transform.position) 
                < fov)
            {
                npc.state = NPC.State.CHASING;
                //if (!hasRolled)
                //{
                //    npc.anim.SetTrigger("Roll");
                //    hasRolled = true;
                //}
                //lastPlayerLoc = player.transform.position;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        RaycastHit hit;
        if (other.tag == "Player"
         && (Physics.Raycast(transform.position,
                player.transform.position - transform.position,
                out hit, 15f)
         && hit.collider.tag != "Player"))
        {
            npc.ResetNPC();
            npc.state = NPC.State.IDLE;
        }
    }

    public float GetDistanceFromPlayer()
    {
        return Vector3.Distance(player.transform.position, transform.position);
    }

    /*private IEnumerator GetPlayerNode()
    {
        yield return new WaitForSeconds(1.0f);

        Node playerNode = AStarGrid.g.GetNodeFromWorldPosition(player.transform.position);
        target = playerNode.worldPosition;

        updatingPlayerPosition = false;
    }*/
}

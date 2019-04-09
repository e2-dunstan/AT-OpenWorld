﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [HideInInspector]
    public NPC npc;
    private GameObject player;

    private float fov = 50;

    private float searchRange;
    private float chaseSpeed = 0.8f;
    private bool hasRolled = false;
    //public bool updatingPlayerPosition = false;

    private float attackRange = 2.0f;

    private Vector3 lastPlayerLoc = Vector3.zero;
    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        searchRange = GetComponent<SphereCollider>().radius;
    }

    public void Chasing()
    {
        if (GetDistanceFromPlayer() > searchRange / 2)
        {
            npc.state = NPC.State.ROAMING;
            hasRolled = false;
            npc.SetNewTarget();
        }
        else if (GetDistanceFromPlayer() < attackRange)
        {
            npc.state = NPC.State.ATTACKING;
        }
        else
        {
            Debug.Log("Chasing");
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                                            transform.rotation, lookRotation,
                                            npc.movementSpeed * Time.deltaTime);
            transform.position += transform.forward * Time.deltaTime * npc.movementSpeed;
        }

        /*if (!updatingPlayerPosition)
        {
            updatingPlayerPosition = true;
            //StartCoroutine(GetPlayerNode());
        }*/
        //Using A* each frame to calculate the path to the player is too slow
    }

    public void Attacking()
    {
        npc.anim.SetTrigger("Attack");

        if (GetDistanceFromPlayer() > attackRange * 1.5f)
        {
            npc.state = NPC.State.CHASING;
        }
    }

    public void Hit()
    {
        if (GetDistanceFromPlayer() < 2.0f)
            player.GetComponent<CatController>().TakeDamage();
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
                if (!hasRolled)
                {
                    npc.anim.SetTrigger("Roll");
                    hasRolled = true;
                }
                lastPlayerLoc = player.transform.position;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        RaycastHit hit;
        if (other.tag == "Player"
         && Physics.Raycast(transform.position,
                player.transform.position - transform.position,
                out hit, 15f)
         && hit.collider.tag != "Player")
        {
            npc.state = NPC.State.ROAMING;
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

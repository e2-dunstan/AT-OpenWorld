using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    private Animator anim;
    private float moveSpeed = 0;

	void Start ()
    {
        anim = GetComponent<Animator>();
	}

	void FixedUpdate ()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        
        if (moveSpeed != 0 && vertical == 0)
        {
            moveSpeed *= 0.9f; //damping
        }
        else
        {
            moveSpeed = vertical;
        }

        if (moveSpeed > 0)
        {
            anim.SetFloat("speed", moveSpeed);
        }
        
	}
}

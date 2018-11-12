using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    private Animator anim;

    private float moveSpeed = 0;
    private bool damping = false;

	void Start ()
    {
        anim = GetComponent<Animator>();
	}

	void FixedUpdate ()
    {
        Movement();
        Rotation();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            anim.speed = 1.5f;
        }
        else
        {
            anim.speed = 1;
        }
    }

    private void Movement()
    {
        float vertical = Input.GetAxis("Vertical");

        if (Input.GetKeyUp(KeyCode.W))
        {
            damping = true;
        }
        if (damping)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, 0, Time.deltaTime * 2);
            if (Input.GetKeyDown(KeyCode.W) || moveSpeed < 0.01f)
            {
                damping = false;
            }
        }
        else
        {
            moveSpeed = vertical;
        }

        anim.SetFloat("speed", moveSpeed);
    }

    void Rotation()
    {
        float horizontal = Input.GetAxis("Horizontal");

        float y = transform.eulerAngles.y;

        y += horizontal * 3;

        transform.eulerAngles = new Vector3(transform.rotation.x, y, 
                                transform.rotation.z);
    }
}

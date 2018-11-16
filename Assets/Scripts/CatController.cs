using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    private Animator anim;

    private float moveSpeed = 0;
    private bool damping = false;

    private bool jumping = false;

	void Start ()
    {
        anim = GetComponent<Animator>();
	}

	void FixedUpdate ()
    {
        Movement();
        Rotation();
        Jump();

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

    private void Rotation()
    {
        float horizontal = Input.GetAxis("Horizontal");

        float y = transform.eulerAngles.y;

        y += horizontal * 3;

        transform.eulerAngles = new Vector3(transform.rotation.x, y, 
                                transform.rotation.z);
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.applyRootMotion = false;
            jumping = true;
            GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (jumping)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 15f))
            {
                anim.applyRootMotion = true;
                jumping = false;
            }
        }
    }
}

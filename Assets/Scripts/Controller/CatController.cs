using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    private int health = 100;
    private bool KO = false;

    private Animator anim;

    private float moveSpeed = 0;
    private bool damping = false;

    //private bool jumping = false;

    private float vertical;
    private float horizontal;

    private float previousRotation = 0;
    public float rotationSpeed = 1.0f;

    void Start ()
    {
        anim = GetComponent<Animator>();
	}

	void FixedUpdate ()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        Movement();
        Rotation();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            anim.speed = 2f;
        }
        else
        {
            anim.speed = 1.5f;
        }

        if (health <= 0 && !KO)
        {
            anim.SetTrigger("KO");
            KO = true;
        }
    }
    private void Movement()
    {
        if (vertical == 0 && horizontal == 0)
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
            moveSpeed = Mathf.Clamp(Mathf.Abs(vertical) + Mathf.Abs(horizontal), 0, 1);
        }

        anim.SetFloat("speed", moveSpeed * 1.2f);
    }
    private void Rotation()
    {
        float theta = Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(horizontal / vertical));

        //Quartile 1
        if (Mathf.Sign(horizontal) == 1 && Mathf.Sign(vertical) == 1)
        {
            //theta = theta;
        }
        //Quartile 2
        else if (Mathf.Sign(horizontal) == 1 && Mathf.Sign(vertical) == -1)
        {
            theta = 180 - theta;
        }
        //Quartile 3
        else if (Mathf.Sign(horizontal) == -1 && Mathf.Sign(vertical) == -1)
        {
            theta -= 180;
        }
        //Quartile 4
        else if (Mathf.Sign(horizontal) == -1 && Mathf.Sign(vertical) == 1)
        {
            theta = -theta;
        }

        theta = float.IsNaN(theta) ? previousRotation : theta;
        
        transform.rotation = Quaternion.Euler(0, 
            Mathf.Lerp(previousRotation, theta, Time.deltaTime * rotationSpeed), 0);

        previousRotation = theta;
    }

    public void TakeDamage()
    {
        GetComponentInChildren<ParticleSystem>().Play();
        anim.SetTrigger("Hit");
        health -= 10;
    }

    //private void OldMovement()
    //{
    //    float vertical = Input.GetAxis("Vertical");

    //    if (Input.GetKeyUp(KeyCode.W))
    //    {
    //        damping = true;
    //    }
    //    if (damping)
    //    {
    //        moveSpeed = Mathf.Lerp(moveSpeed, 0, Time.deltaTime * 2);
    //        if (Input.GetKeyDown(KeyCode.W) || moveSpeed < 0.01f)
    //        {
    //            damping = false;
    //        }
    //    }
    //    else
    //    {
    //        moveSpeed = vertical;
    //    }

    //    anim.SetFloat("speed", moveSpeed);
    //}

    //private void OldRotation()
    //{
    //    float horizontal = Input.GetAxis("Horizontal");

    //    float y = transform.eulerAngles.y;

    //    y += horizontal * 3;

    //    transform.eulerAngles = new Vector3(transform.rotation.x, y, 
    //                            transform.rotation.z);
    //}
    //private void Jump()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        anim.applyRootMotion = false;
    //        jumping = true;
    //        GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (jumping)
    //    {
    //        RaycastHit hit;
    //        if (Physics.Raycast(transform.position, Vector3.down, out hit, 15f))
    //        {
    //            anim.applyRootMotion = true;
    //            jumping = false;
    //        }
    //    }
    //}
}

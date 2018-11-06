using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        GROUNDED, IN_AIR
    }
    private PlayerState state;

    private Rigidbody rb; 
    private GameObject mainCamera;

    public float speed;

    private float xInput;
    private float yInput;

    private float prevRot = 0;
    private int jumpHeight = 2;

    private bool doubleJumped;

    void Start()
    {
        mainCamera = Camera.main.gameObject;
        rb = GetComponent<Rigidbody>();
    }

    void GetInputs()
    {
        xInput = Input.GetAxis("X Axis");
        yInput = -Input.GetAxis("Y Axis");
    }

    void FixedUpdate()
    {
        GetInputs();

        switch (state)
        {
            case PlayerState.GROUNDED:
                {
                    Movement();
                    //Jump();
                    //Rotation();
                    break;
                }
            case PlayerState.IN_AIR:
                {
                    Rotation();
                    AirMovement();
                    break;
                }
            default:
                break;
        }
        mainCamera.transform.LookAt(transform.position);
    }

    void Movement()
    {
        Vector3 movement = new Vector3(xInput, 0, yInput);

        //float calculatedSpeed = Mathf.Sqrt((xInput * xInput) + (yInput * yInput));
        rb.AddForce(transform.forward + movement.normalized);

        //Debug.Log("Movement " + movement);
        //rb.MovePosition(transform.position + movement * Time.deltaTime);
        //transform.LookAt(transform.position + movement);
        /*
        float calculated_speed = Mathf.Sqrt((xInput * xInput) + (yInput * yInput));
        if (calculated_speed > 0)
        {
            Vector3 movement = new Vector3(xInput, 0, yInput);
            rb.AddForce(movement * 10);
           //rb.velocity = transform.forward * Mathf.Abs(calculated_speed);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }*/
    }

    void Rotation()
    {
        float theta = Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(xInput / yInput));

        //Quartile 1
        if (Mathf.Sign(xInput) == 1 && Mathf.Sign(yInput) == 1)
        {
            //theta = theta;
        }
        //Quartile 2
        else if (Mathf.Sign(xInput) == 1 && Mathf.Sign(yInput) == -1)
        {
            theta = 180 - theta;
        }
        //Quartile 3
        else if (Mathf.Sign(xInput) == -1 && Mathf.Sign(yInput) == -1)
        {
            theta -= 180;
        }
        //Quartile 4
        else if (Mathf.Sign(xInput) == -1 && Mathf.Sign(yInput) == 1)
        {
            theta = -theta;
        }

        theta = float.IsNaN(theta) ? prevRot : theta;
        prevRot = theta;

        theta += mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, theta, 0);
    }

    void Jump()
    {
        if (Input.GetButton("Jump"))
        {
            state = PlayerState.IN_AIR;
            doubleJumped = false;
            rb.velocity = new Vector3(0, 0, 0);
            rb.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
            transform.position += new Vector3(0, 0.1f, 0);
        }
    }

    void AirMovement()
    {
        float calculated_speed = Mathf.Sqrt((xInput * xInput) + (yInput * yInput));

        rb.AddRelativeForce(Vector3.forward * calculated_speed * 10000);

        if (Input.GetButtonDown("Jump") && !doubleJumped)
        {
            doubleJumped = true;
            rb.velocity = new Vector3(0, 0, 0);
            rb.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state == PlayerState.IN_AIR)
        {
            state = PlayerState.GROUNDED;
        }
    }
}

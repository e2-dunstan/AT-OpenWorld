using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        GROUNDED, IN_AIR
    }
    private PlayerState state;

    private Rigidbody rb;
    private GameObject main_camera;

    public float speed;

    public float x_movement;
    public float y_movement;

    private float previous_rotation = 0;
    private int jump_height = 350;

    private bool double_jumped;

    void Start()
    {
        main_camera = Camera.main.gameObject;
        rb = GetComponent<Rigidbody>();
    }

    void Inputs()
    {
        x_movement = Input.GetAxis("X Axis");
        y_movement = -Input.GetAxis("Y Axis");
    }

    void FixedUpdate()
    {
        Inputs();

        switch (state)
        {
            case PlayerState.GROUNDED:
                {
                    Movement();
                    Jump();
                    Rotation();
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
        main_camera.transform.LookAt(transform.position);
    }

    void Movement()
    {
        float calculated_speed = Mathf.Sqrt((x_movement * x_movement) + (y_movement * y_movement));
        if (calculated_speed > 0)
        {
            Vector3 movement = new Vector3(x_movement, 0, y_movement);
            rb.AddForce(movement * Time.deltaTime * 100);
           //rb.velocity = transform.forward * Mathf.Abs(calculated_speed);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    void Rotation()
    {
        float theta = Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(x_movement / y_movement));

        //Quartile 1
        if (Mathf.Sign(x_movement) == 1 && Mathf.Sign(y_movement) == 1)
        {
            //theta = theta;
        }
        //Quartile 2
        else if (Mathf.Sign(x_movement) == 1 && Mathf.Sign(y_movement) == -1)
        {
            theta = 180 - theta;
        }
        //Quartile 3
        else if (Mathf.Sign(x_movement) == -1 && Mathf.Sign(y_movement) == -1)
        {
            theta -= 180;
        }
        //Quartile 4
        else if (Mathf.Sign(x_movement) == -1 && Mathf.Sign(y_movement) == 1)
        {
            theta = -theta;
        }

        theta = float.IsNaN(theta) ? previous_rotation : theta;
        previous_rotation = theta;

        theta += main_camera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, theta, 0);
    }

    void Jump()
    {
        if (Input.GetButton("Jump"))
        {
            state = PlayerState.IN_AIR;
            double_jumped = false;
            rb.velocity = new Vector3(0, 0, 0);
            rb.AddForce(new Vector3(0, jump_height, 0), ForceMode.Impulse);
            transform.position += new Vector3(0, 0.1f, 0);
        }
    }

    void AirMovement()
    {
        float calculated_speed = Mathf.Sqrt((x_movement * x_movement) + (y_movement * y_movement));

        rb.AddRelativeForce(Vector3.forward * calculated_speed * 10000);

        if (Input.GetButtonDown("Jump") && !double_jumped)
        {
            double_jumped = true;
            rb.velocity = new Vector3(0, 0, 0);
            rb.AddForce(new Vector3(0, jump_height, 0), ForceMode.Impulse);
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

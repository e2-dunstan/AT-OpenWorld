using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate ()
    {
        Vector3 dirVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        Vector3 newPos = transform.position + dirVector * Time.deltaTime;

        GetComponent<Rigidbody>().MovePosition(newPos);

        transform.LookAt(new Vector3(newPos.x, transform.position.y, newPos.z));
    }
}

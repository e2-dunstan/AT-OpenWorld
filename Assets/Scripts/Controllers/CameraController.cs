using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;

    private int travel;
    private int scrollSpeed = 40;

    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	void LateUpdate ()
    {
        transform.position = player.transform.position;

        float m = Input.GetAxis("Mouse ScrollWheel");
        if (m > 0f && travel > -15)
        {
            travel -= scrollSpeed;
            transform.GetChild(0).Translate(0, 0, 1 * scrollSpeed * Time.deltaTime, Space.Self);
        }
        else if (m < 0f && travel < 500)
        {
            travel += scrollSpeed;
            transform.GetChild(0).Translate(0, 0, -1 * scrollSpeed * Time.deltaTime, Space.Self);
        }

    }
}

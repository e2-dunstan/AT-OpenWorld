using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;

	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	void LateUpdate ()
    {
        transform.position = player.transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, player.transform.rotation, Time.deltaTime * 4);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearestKittenArrow : MonoBehaviour
{
    public List<Transform> kittens;
    private Transform player;
    private Vector3 nearestPos = Vector3.zero;
    private RectTransform rect;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rect = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        GetNearestKitten();
        float angle = Vector3.Angle(Vector3.left, nearestPos - player.position);
        //Get correct heading rather than acute
        if (nearestPos.z > player.position.z)
        {
            angle = 360 - angle;
        }
        rect.eulerAngles = new Vector3(0, 0, angle);
    }

    private void GetNearestKitten()
    {
        Transform nearest = GetNearest();
        if (nearest != null)
        {
            nearestPos = nearest.position;
        }
        else
        {
            Debug.LogWarning("Kitten cannot be found!");
        }
    }

    Transform GetNearest()
    {
        float prevDistance = 0;
        Transform nearest = null;
        foreach(Transform kitten in kittens)
        {
            if (kitten.GetComponentInChildren<KittenController>().following)
            {
                continue;
            }
            else if (prevDistance == 0 
                || prevDistance > Vector3.Distance(kitten.position, player.position))
            {
                prevDistance = Vector3.Distance(kitten.position, player.position);
                nearest = kitten;
            }
        }
        return nearest;
    }
}

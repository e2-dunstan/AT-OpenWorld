using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    struct Coordinates
    {
        public float x;
        public float z;
    }
    private Coordinates[] coordinates;

    public Vector2 origin;
    public Vector2 size;
    private Vector2 actualSize;

    public int gridSpacing = 100;

    private void OnDrawGizmosSelected()
    {
        Vector2 actualSize = size - origin;
        Gizmos.color = new Color(1, 0, 0, 1);

        //0,0
        Vector3 minXminZ = new Vector3(origin.x, 10, origin.y);
        //0,1
        Vector3 minXmaxZ = new Vector3(origin.x, 10, actualSize.y);
        //1,0
        Vector3 maxXminZ = new Vector3(actualSize.x, 10, origin.y);
        //1,1
        Vector3 maxXmaxZ = new Vector3(actualSize.x, 10, actualSize.y);

        Gizmos.DrawLine(minXminZ, minXmaxZ);
        Gizmos.DrawLine(minXminZ, maxXminZ);
        Gizmos.DrawLine(maxXminZ, maxXmaxZ);
        Gizmos.DrawLine(minXmaxZ, maxXmaxZ);

        //for (int i = 0; i < (actualSize.x / gridSpacing); i += gridSpacing)
        //{
        //    Vector3 lineX0 = new Vector3(gridSpacing * i, 0, actualSize.y);
        //    Gizmos.DrawLine(minXminZ, minXmaxZ);
        //}
        //for (int i = 0; i < (actualSize.y / gridSpacing); i += gridSpacing)
        //{
        //    Vector3 lineX0 = new Vector3(gridSpacing * i, 0, actualSize.)
        //    Gizmos.DrawLine(minXminZ, minXmaxZ);
        //}

    }

    void Start ()
    {
        actualSize = size - origin;
	}
}

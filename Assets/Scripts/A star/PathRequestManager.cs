using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    struct PathRequest
    {
        public Vector3 start;
        public Vector3 end;
        public Action<Vector3[], bool> callback;

        //Constructor
        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _cllbck)
        {
            start = _start;
            end = _end;
            callback = _cllbck;
        }
    }

    private PathRequest currentPathRequest;
    private Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();

    public static PathRequestManager prm;
    [HideInInspector]
    public Pathfinder pathfinder;

    private bool isProcessingPath;

    private void Awake()
    {
        if (prm == null)
            prm = this;

        pathfinder = GetComponent<Pathfinder>();
    }

    //WRITE ABOUT ACTIONS IN THE REPORT
    public static void RequestPath(Vector3 _pathStart, Vector3 _pathEnd, 
        Action<Vector3[], bool> _callback)
    {
        PathRequest newRequest = new PathRequest(_pathStart, _pathEnd, _callback);
        prm.pathRequestQueue.Enqueue(newRequest);
        prm.TryProcessNextPath();
    }

    private void TryProcessNextPath()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinder.StartFindPathCoroutine(currentPathRequest.start, currentPathRequest.end);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNextPath();
    }

    public void ClearQueue()
    {
        pathRequestQueue.Clear();
    }
}

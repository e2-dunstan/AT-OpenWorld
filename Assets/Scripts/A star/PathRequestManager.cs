using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    private Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    private PathRequest currentPathRequest;

    private static PathRequestManager prm;
    private Pathfinder pathfinder;

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
        prm.TryProcessNext();
    }

    private void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinder.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        //Constructor
        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _cllbck)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _cllbck;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PathRequestManager3D : MonoBehaviour {
        readonly Queue<PathRequest> pathRequestQueue = new();
        PathRequest currentPathRequest;

        public static PathRequestManager3D instance;
        PathFinding3D pathFinding;
        bool isProcessingPath;

        void Awake() {
            if (instance == null)
                instance = this;
            else Destroy(gameObject);
            pathFinding = GetComponent<PathFinding3D>();
        }

        public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback) {
            PathRequest newRequest = new(pathStart, pathEnd, callback);
            instance.pathRequestQueue.Enqueue(newRequest);
            instance.TryProcessNext();
        }

        void TryProcessNext() {
            if (!isProcessingPath && pathRequestQueue.Count > 0) {
                currentPathRequest = pathRequestQueue.Dequeue();
                isProcessingPath = true;
                pathFinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
            }
        }

        public void FinishedProcessingPath(Vector3[] path, bool success) {
            //Debug.Log("현재 경로 탐색 종료");
            if (path.Length > 0) {
                //Debug.Log("콜백");
                currentPathRequest.callback(path, success);
            }
            isProcessingPath = false;
            TryProcessNext();
        }


        struct PathRequest {
            public Vector3 pathStart;
            public Vector3 pathEnd;
            public Action<Vector3[], bool> callback;

            public PathRequest(Vector3 _pathStart, Vector3 _pathEnd, Action<Vector3[], bool> _callback) {
                pathStart = _pathStart;
                pathEnd = _pathEnd;
                callback = _callback;
            }

        }
    }
}
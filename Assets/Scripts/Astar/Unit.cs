using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace KBH {
    public class Unit : MonoBehaviour {
        public Transform target;
        float speed = 1.5f;
        Vector3[] path;
        Vector3 currentWaypoint;
        int targetIndex;
        float attackDistance = 1f;

        void Awake() {
            currentWaypoint = transform.position;
        }

        void Update() {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance > attackDistance) {
                if (WaypointProgressingStatus())
                    PathRequestManager3D.RequestPath(transform.position, target.position, OnPathFound);
            } else
                Debug.Log("공격!!!");
        }

        bool WaypointProgressingStatus() {
            return Vector3.Distance(currentWaypoint ,transform.position) < 0.5f;
        }

        public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
            //Debug.Log("경로 탐색 성공 : " + pathSuccessful);
            if (pathSuccessful) {
                path = newPath;
                StopCoroutine(nameof(FollowPath));
                StartCoroutine(nameof(FollowPath));
            }
        }

        IEnumerator FollowPath() {
            //Debug.Log("경로 이동");
            currentWaypoint = path[0];
            while (true) {
                //Debug.Log("현재 웨이포인트 : " + currentWaypoint + " 로 이동 중");
                if (transform.position == currentWaypoint) {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                        yield break;
                    currentWaypoint = path[targetIndex];
                }

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public Transform target;
    float speed = 3f;
    Vector3[] path;
    int targetIndex;
    float attackDistance = 1f;

    void Update() {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > attackDistance)
            PathRequestManager3D.RequestPath(transform.position, target.position, OnPathFound);
        else
            Debug.Log("공격!!!");
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
        Vector3 currentWaypoint = path[0];
        while (true) {
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

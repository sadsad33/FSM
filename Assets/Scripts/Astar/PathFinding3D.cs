using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding3D : MonoBehaviour {
    PathRequestManager3D requestManager;
    Grid3D grid;

    private void Awake() {
        requestManager = GetComponent<PathRequestManager3D>();
        grid = GetComponent<Grid3D>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();
        //Debug.Log("경로 탐색 시작");
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        //Debug.Log("시작 노드 : ");
        //Debug.Log("시작 월드 좌표 : " + startPos);
        Node3D startNode = grid.GetNodeFromWorldPoint(startPos);
        //Debug.Log(startNode.isWalkable);
        //Debug.Log("시작 그리드 좌표 : (" + startNode.gridX + " , " + startNode.gridY + " , " + startNode.gridZ + ")");

        //Debug.Log("도착 노드 : ");
        //Debug.Log("타겟 월드좌표: " + targetPos);
        Node3D targetNode = grid.GetNodeFromWorldPoint(targetPos);
        //Debug.Log(targetNode.isWalkable);
        //Debug.Log("타겟 그리드 좌표 : (" + targetNode.gridX + ", " + targetNode.gridY + " , " + targetNode.gridZ + ")");

        if (startNode.isWalkable && targetNode.isWalkable) {
            Heap<Node3D> openList = new(grid.MaxSize);
            HashSet<Node3D> closedList = new();
            openList.Add(startNode);

            while (openList.Count > 0) {
                Node3D currentNode = openList.RemoveFirst();
                closedList.Add(currentNode);

                if (currentNode == targetNode) {
                    //sw.Stop();
                    //print("Path Found : " + sw.ElapsedMilliseconds + "ms");
                    pathSuccess = true;
                    break;
                }

                foreach (Node3D neighbour in grid.GetNeighbours(currentNode)) {
                    if (!neighbour.isWalkable || closedList.Contains(neighbour)) continue;

                    int newCurrentToNeighbourCost = currentNode.gCost + GetDistanceCost(currentNode, neighbour) + neighbour.movementPenalty;
                    if (newCurrentToNeighbourCost < neighbour.gCost || !openList.Contains(neighbour)) {
                        //Debug.Log("이웃 그리드 좌표 : " + neighbour.gridX + ", " + neighbour.gridY + ", " + neighbour.gridZ);
                        neighbour.gCost = newCurrentToNeighbourCost;
                        neighbour.hCost = GetDistanceCost(neighbour, targetNode);
                        neighbour.parentNode = currentNode;

                        if (!openList.Contains(neighbour)) openList.Add(neighbour);
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess) {
            waypoints = RetracePath(startNode, targetNode);
            //Debug.Log("경로 길이 : " + waypoints.Length);
        }
        //Debug.Log(waypoints);
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    // 탐색된 경로에 LadderNode 가 단 하나라도 포함되어 있을 경우 경로를 재구성함
    // 사다리를 이용하는 경우 사다리의 꼭대기 지점과 사다리의 바닥 지점을 반드시 통과하도록 해야함
    // 문제는 사다리의 꼭대기 지점과 바닥지점을 어떻게 경로에 추가할 것인가
    // 경로에 포함된 사다리 노드로부터 위 아래를 검사하여 사다리 노드가 끝나는 지점을 각각 꼭대기, 바닥지점으로 설정한후 경로에 추가해주는 방식은 깔끔하게 실패
    Vector3[] gizmoPoints;
    Vector3[] RetracePath(Node3D startNode, Node3D endNode) {
        List<Node3D> path = new List<Node3D>();
        Node3D currentNode = endNode;

        Node3D ladderStartNode = null;
        Node3D ladderEndNode = null;
        int ladderStartNodeIndex = -1;
        int ladderEndNodeIndex = -1;
        Vector3 ladderDirection = Vector3.zero;
        int index = 0;

        // ladderNode 가 시작되는 지점과 끝나는지점 모두 찾아야함
        while (currentNode != startNode) {
            // 경로를 역추적하므로 경로 상에서 사다리의 이용을 끝내는 지점부터 탐색될 것
            if (ladderEndNode == null && currentNode.parentNode.isLadder) {
                ladderEndNode = currentNode.parentNode;
                ladderEndNodeIndex = index + 1;
            }
            if (currentNode.isLadder && !currentNode.parentNode.isLadder) {
                if (ladderEndNode != null) {
                    ladderStartNode = currentNode;
                    ladderStartNodeIndex = index + 1;
                    ladderDirection = new(ladderEndNode.gridX - currentNode.gridX, ladderEndNode.gridY - currentNode.gridY, ladderEndNode.gridZ - currentNode.gridZ);
                }
            }
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
            index++;
        }
        Debug.Log("이전 : " + path.Count);
        AddLadderNode(ladderEndNode, ladderEndNodeIndex, ladderStartNode, ladderStartNodeIndex, ladderDirection, path);
        Debug.Log("이후 : " + path.Count);

        //기즈모 확인용
        grid.path = path;

        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        gizmoPoints = waypoints;
        return waypoints;
    }

    List<Node3D> checkList;
    public Node3D ladderTop = null;
    public Node3D ladderBottom = null;
    void AddLadderNode(Node3D ladderEndNode, int ladderEndIndex, Node3D ladderStartNode, int ladderStartIndex, Vector3 ladderPathDirection, List<Node3D> path) {
        if (ladderStartNode == null) return;
        if (ladderPathDirection == Vector3.zero) return;

        checkList = new();
        Node3D upperNode = ladderStartNode;
        Node3D lowerNode = ladderStartNode;
        int upperOffset = 1;
        int lowerOffset = 1;

        checkList.Add(ladderStartNode);
        int iterationCnt = 0;
        while (true) {
            if (iterationCnt > 1000) {
                Debug.Log("반복 횟수 초과");
                break;
            }
            upperNode = grid.GetNode(ladderStartNode.gridX, ladderStartNode.gridY + upperOffset, ladderStartNode.gridZ);

            if (!upperNode.isLadder) break;
            else {
                ladderTop = upperNode;
                checkList.Add(upperNode);
            }
            upperOffset++;
            iterationCnt++;
        }

        iterationCnt = 0;
        while (true) {
            if (iterationCnt > 1000) {
                Debug.Log("반복 횟수 초과");
                break;
            }
            lowerNode = grid.GetNode(ladderStartNode.gridX, ladderStartNode.gridY - lowerOffset, ladderStartNode.gridZ);

            if (!lowerNode.isLadder) break;
            else {
                ladderBottom = lowerNode;
                checkList.Add(lowerNode);
            }
            lowerOffset++;
            iterationCnt++;
        }

        if (ladderPathDirection.y > 0) {
            path.Insert(ladderStartIndex, ladderBottom);
            path.Insert(ladderEndIndex, ladderTop);
        } else {
            path.Insert(ladderStartIndex, ladderTop);
            path.Insert(ladderEndIndex, ladderBottom);
        }
    }

    // 경로를 간략화 함
    Vector3[] SimplifyPath(List<Node3D> path) {
        List<Vector3> waypoints = new();
        Vector3 directionOld = Vector3.zero;

        for (int i = 1; i < path.Count; i++) {
            Vector3 directionNew = new(path[i - 1].gridX - path[i].gridX,
                path[i - 1].gridY - path[i].gridY,
                path[i - 1].gridZ - path[i].gridZ);
            // 이동해야 하는 방향이 일치하지않는다면 웨이포인트에 해당 좌표를 추가한다.
            if (directionNew != directionOld) {
                waypoints.Add(path[i].worldPos);
                //grid.wayPoints Add(path[i].worldPos);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }



    int GetDistanceCost(Node3D nodeA, Node3D nodeB) {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        int distZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);
        //int upwardDirection;
        //if (nodeB.gridY > nodeA.gridY) upwardDirection = 1;
        //else if (nodeB.gridY == nodeA.gridY) upwardDirection = 0;
        //else upwardDirection = -1;
        int minDist = Mathf.Min(distX, distY, distZ);
        int maxDist = Mathf.Max(distX, distY, distZ);
        int midDist = distX + distY + distZ - (minDist + maxDist);

        //// 높이 차이에 따른 추가 가중치 적용 (경사면 고려)
        //float slopeFactor = 1.5f; // 높이 차이에 대한 가중치
        //int heightPenalty = Mathf.RoundToInt(distY * slopeFactor * 10);

        ////3D 대각선(√3), 2D 대각선(√2), 직선 이동(1) 비용 적용 +높이 패널티 추가
        //return 17 * minDist + 14 * (midDist - minDist) + 10 * (maxDist - midDist) + heightPenalty;
        return 17 * minDist + 14 * (midDist - minDist) + 10 * (maxDist - midDist);
    }

    void OnDrawGizmos() {
        if (gizmoPoints != null) {
            Gizmos.color = Color.magenta;
            for (int i = 0; i < gizmoPoints.Length; i++) {
                Gizmos.DrawSphere(gizmoPoints[i], 0.5f);
            }
        }

        if (checkList != null) {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < checkList.Count; i++) {
                Gizmos.DrawWireCube(checkList[i].worldPos, Vector3.one);
            }
        }

        Gizmos.color = Color.blue;
        if (ladderTop != null)
            Gizmos.DrawSphere(ladderTop.worldPos, 0.3f);
        if (ladderBottom != null)
            Gizmos.DrawSphere(ladderBottom.worldPos, 0.3f);
    }
}

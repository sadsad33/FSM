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
        //Debug.Log("��� Ž�� ����");
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        //Debug.Log("���� ��� : ");
        //Debug.Log("���� ���� ��ǥ : " + startPos);
        Node3D startNode = grid.GetNodeFromWorldPoint(startPos);
        //Debug.Log(startNode.isWalkable);
        //Debug.Log("���� �׸��� ��ǥ : (" + startNode.gridX + " , " + startNode.gridY + " , " + startNode.gridZ + ")");

        //Debug.Log("���� ��� : ");
        //Debug.Log("Ÿ�� ������ǥ: " + targetPos);
        Node3D targetNode = grid.GetNodeFromWorldPoint(targetPos);
        //Debug.Log(targetNode.isWalkable);
        //Debug.Log("Ÿ�� �׸��� ��ǥ : (" + targetNode.gridX + ", " + targetNode.gridY + " , " + targetNode.gridZ + ")");

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
                        //Debug.Log("�̿� �׸��� ��ǥ : " + neighbour.gridX + ", " + neighbour.gridY + ", " + neighbour.gridZ);
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
            //Debug.Log("��� ���� : " + waypoints.Length);
        }
        //Debug.Log(waypoints);
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    // Ž���� ��ο� LadderNode �� �� �ϳ��� ���ԵǾ� ���� ��� ��θ� �籸����
    // ��ٸ��� �̿��ϴ� ��� ��ٸ��� ����� ������ ��ٸ��� �ٴ� ������ �ݵ�� ����ϵ��� �ؾ���
    // ������ ��ٸ��� ����� ������ �ٴ������� ��� ��ο� �߰��� ���ΰ�
    // ��ο� ���Ե� ��ٸ� ���κ��� �� �Ʒ��� �˻��Ͽ� ��ٸ� ��尡 ������ ������ ���� �����, �ٴ��������� �������� ��ο� �߰����ִ� ����� ����ϰ� ����
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

        // ladderNode �� ���۵Ǵ� ������ ���������� ��� ã�ƾ���
        while (currentNode != startNode) {
            // ��θ� �������ϹǷ� ��� �󿡼� ��ٸ��� �̿��� ������ �������� Ž���� ��
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
        Debug.Log("���� : " + path.Count);
        AddLadderNode(ladderEndNode, ladderEndNodeIndex, ladderStartNode, ladderStartNodeIndex, ladderDirection, path);
        Debug.Log("���� : " + path.Count);

        //����� Ȯ�ο�
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
                Debug.Log("�ݺ� Ƚ�� �ʰ�");
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
                Debug.Log("�ݺ� Ƚ�� �ʰ�");
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

    // ��θ� ����ȭ ��
    Vector3[] SimplifyPath(List<Node3D> path) {
        List<Vector3> waypoints = new();
        Vector3 directionOld = Vector3.zero;

        for (int i = 1; i < path.Count; i++) {
            Vector3 directionNew = new(path[i - 1].gridX - path[i].gridX,
                path[i - 1].gridY - path[i].gridY,
                path[i - 1].gridZ - path[i].gridZ);
            // �̵��ؾ� �ϴ� ������ ��ġ�����ʴ´ٸ� ��������Ʈ�� �ش� ��ǥ�� �߰��Ѵ�.
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

        //// ���� ���̿� ���� �߰� ����ġ ���� (���� ���)
        //float slopeFactor = 1.5f; // ���� ���̿� ���� ����ġ
        //int heightPenalty = Mathf.RoundToInt(distY * slopeFactor * 10);

        ////3D �밢��(��3), 2D �밢��(��2), ���� �̵�(1) ��� ���� +���� �г�Ƽ �߰�
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

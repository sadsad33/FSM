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
            Heap<Node3D> openList = new Heap<Node3D>(grid.MaxSize);
            HashSet<Node3D> closedList = new HashSet<Node3D>();
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

    Vector3[] RetracePath(Node3D startNode, Node3D endNode) {
        List<Node3D> path = new List<Node3D>();
        Node3D currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }
        grid.path = path;
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    // ��θ� ����ȭ ��
    Vector3[] SimplifyPath(List<Node3D> path) {
        List<Vector3> waypoints = new List<Vector3>();
        Vector3 directionOld = Vector3.zero;

        for (int i = 1; i < path.Count; i++) {
            Vector3 directionNew = new(path[i - 1].gridX - path[i].gridX,
                path[i - 1].gridY - path[i].gridY,
                path[i - 1].gridZ - path[i].gridZ);
            // �̵��ؾ� �ϴ� ������ ��ġ�����ʴ´ٸ� ��������Ʈ�� �ش� ��ǥ�� �߰��Ѵ�.
            if (directionNew != directionOld)
                waypoints.Add(path[i].worldPos);
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistanceCost(Node3D nodeA, Node3D nodeB) {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        int distZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);

        int minDist = Mathf.Min(distX, distY, distZ);
        int maxDist = Mathf.Max(distX, distY, distZ);
        int midDist = distX + distY + distZ - (minDist + maxDist);

        // ���� ���̿� ���� �߰� ����ġ ���� (���� ���)
        //float slopeFactor = 1.5f; // ���� ���̿� ���� ����ġ
        //int heightPenalty = Mathf.RoundToInt(distY * slopeFactor * 10);

        // 3D �밢��(��3), 2D �밢��(��2), ���� �̵�(1) ��� ���� + ���� �г�Ƽ �߰�
        //return 17 * minDist + 14 * (midDist - minDist) + 10 * (maxDist - midDist) + heightPenalty;
        return 17 * minDist + 14 * (midDist - minDist) + 10 * (maxDist - midDist);
    }
}

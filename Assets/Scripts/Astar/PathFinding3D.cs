using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
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
            if (!startNode.isWalkable && Physics.CheckSphere(startPos, 0.5f, 9)) {
                startNode.isWalkable = true;
            }

            //Debug.Log("���� ��� : ");
            //Debug.Log("Ÿ�� ������ǥ: " + targetPos);
            Node3D targetNode = grid.GetNodeFromWorldPoint(targetPos);
            //Debug.Log(targetNode.isWalkable);
            //Debug.Log("Ÿ�� �׸��� ��ǥ : (" + targetNode.gridX + ", " + targetNode.gridY + " , " + targetNode.gridZ + ")");
            if (!targetNode.isWalkable && Physics.CheckSphere(targetPos, 0.5f, 9)) {
                targetNode.isWalkable = true;
            }

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

                            if (!openList.Contains(neighbour)) {
                                openList.Add(neighbour);
                            }
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
        Vector3[] gizmoPoints;
        Node3D markingLadder;
        Vector3[] RetracePath(Node3D startNode, Node3D endNode) {
            List<Node3D> path = new();
            Node3D currentNode = endNode;
            Node3D currentLadderLast = null;
            Node3D currentLadderFirst = null;
            int indexCurrentLadderLast = -1;
            int indexCurrentLadderFirst = -1;

            bool ladderStart = false;
            if (startNode.isLadder) {
                //Debug.Log("��ٸ����� ����");
                ladderStart = true;
            }
            int index = 0;

            // ��� �߰��� ��ٸ���尡 �ִ°��
            // ��ΰ� ��ٸ����� ���۵Ǵ� ���
            // ��ΰ� ��ٸ����� ������ ���
            // ����� ���۰� ���� ��ٸ� �ΰ��

            // ladderNode �� ���۵Ǵ� ������ ���������� ��� ã�ƾ���
            while (currentNode != startNode) {
                path.Add(currentNode);
                if (currentNode.isLadder) {
                    if (currentLadderLast == null) {
                        currentLadderLast = currentNode;
                        indexCurrentLadderLast = index;
                    }
                    if (currentLadderFirst == null) {
                        if (!currentNode.parentNode.isLadder || currentNode.parentNode == null) {
                            currentLadderFirst = currentNode;
                            indexCurrentLadderFirst = index;
                        }
                    }
                }

                if (currentLadderLast != null && currentLadderFirst != null) {
                    //Debug.Log(path.Count);
                    path = AddLadderNode(currentLadderFirst, indexCurrentLadderLast, indexCurrentLadderFirst, currentLadderLast, ladderStart, path);
                    currentLadderLast = null;
                    currentLadderFirst = null;
                }

                currentNode = currentNode.parentNode;
                index++;
            }

            //����� Ȯ�ο�
            grid.path = path;

            Vector3[] waypoints = SimplifyPath(path);
            Array.Reverse(waypoints);
            gizmoPoints = waypoints;
            return waypoints;
        }

        List<Node3D> AddLadderNode(Node3D currentLadderFirst, int indexCurrentLadderLast, int indexCurrentLadderFirst, Node3D currentLadderLast, bool ladderStart, List<Node3D> path) {
            if (currentLadderFirst.ladder == null) {
                Debug.LogError("��� ��忡 ��ٸ� ������ ����");
                return path;
            } else {
                // ��ٸ��� ���� ������ �˻�
                bool descending = false;
                if (ladderStart) {
                    if (currentLadderFirst.worldPos.y > currentLadderLast.worldPos.y) descending = true;
                }
                Ladder startingLadder = currentLadderFirst.ladder;
                Ladder otherSideLadder = null;
                Vector3 currentPosition = currentLadderFirst.worldPos;
                Node3D startingLadderNode = new(true, true, currentPosition);

                int searchingDirection;
                if (startingLadder.isTop) {
                    searchingDirection = -1;
                    //Debug.Log("����� ����");
                } else {
                    searchingDirection = 1;
                    //Debug.Log("�ٴ� ����");
                }


                while (true) {
                    currentPosition = new(currentPosition.x, currentPosition.y + searchingDirection, currentPosition.z);
                    Node3D currentNode = grid.GetNodeFromWorldPoint(currentPosition);
                    if (!currentNode.isLadder) {
                        Debug.LogError("Ž�� ���� ���� : ��ٸ��� ���");
                        return path;
                    } else if (!currentNode.ladder.isTop == startingLadder.isTop) {
                        otherSideLadder = currentNode.ladder;
                        break;
                    }
                }
                List<Node3D> newPath = new();
                //Debug.Log("���� Path ���� : " + path.Count);
                for (int i = 0; i < indexCurrentLadderLast; i++) {
                    newPath.Add(path[i]);
                }
                //if (!ladderStart) {
                //    newPath.Add(new Node3D(true, true, otherSideLadder.GetClimbingStartPosition()));
                //    newPath.Add(new Node3D(true, true, otherSideLadder.GetInteractionStartingPosition()));
                //    newPath.Add(new Node3D(true, true, startingLadder.GetClimbingStartPosition()));
                //    newPath.Add(new Node3D(true, true, startingLadder.GetInteractionStartingPosition()));
                //} else {
                //    if (descending) {
                //        if (!otherSideLadder.isTop) {
                //            newPath.Add(new Node3D(true, true, otherSideLadder.GetClimbingStartPosition()));
                //            newPath.Add(new Node3D(true, true, otherSideLadder.GetInteractionStartingPosition()));
                //        }
                //        if (currentLadderFirst.worldPos.y >= startingLadder.GetClimbingStartPosition().y) {
                //            newPath.Add(new Node3D(true, true, startingLadder.GetClimbingStartPosition()));
                //        }
                //        if (currentLadderFirst.worldPos.y >= startingLadder.GetInteractionStartingPosition().y) {
                //            newPath.Add(new Node3D(true, true, startingLadder.GetInteractionStartingPosition()));
                //        }
                //    } else {
                //        if (otherSideLadder.isTop) {
                //            newPath.Add(new Node3D(true, true, otherSideLadder.GetClimbingStartPosition()));
                //            newPath.Add(new Node3D(true, true, otherSideLadder.GetInteractionStartingPosition()));
                //        }
                //        if (currentLadderFirst.worldPos.y <= startingLadder.GetClimbingStartPosition().y) {
                //            newPath.Add(new Node3D(true, true, startingLadder.GetClimbingStartPosition()));
                //        }
                //        if (currentLadderFirst.worldPos.y <= startingLadder.GetInteractionStartingPosition().y) {
                //            newPath.Add(new Node3D(true, true, startingLadder.GetInteractionStartingPosition()));
                //        }
                //    }
                //}
                newPath.Add(new Node3D(true, true, otherSideLadder.GetClimbingStartPosition()));
                newPath.Add(new Node3D(true, true, otherSideLadder.GetInteractionStartingPosition()));
                newPath.Add(new Node3D(true, true, startingLadder.GetClimbingStartPosition()));
                newPath.Add(new Node3D(true, true, startingLadder.GetInteractionStartingPosition()));
                return newPath;
            }
        }

        // ��θ� ����ȭ ��
        Vector3[] SimplifyPath(List<Node3D> path) {
            List<Vector3> waypoints = new();
            Vector3 directionOld = Vector3.zero;

            for (int i = 1; i < path.Count; i++) {
                if (path[i].isLadder) {
                    waypoints.Add(path[i].worldPos);
                } else {
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

            if (markingLadder != null) {
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(markingLadder.worldPos, 0.5f);
            }
        }
    }
}
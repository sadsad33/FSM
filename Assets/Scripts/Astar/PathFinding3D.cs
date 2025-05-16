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
            //Debug.Log("경로 탐색 시작");

            Vector3[] waypoints = new Vector3[0];
            bool pathSuccess = false;

            //Debug.Log("시작 노드 : ");
            //Debug.Log("시작 월드 좌표 : " + startPos);
            Node3D startNode = grid.GetNodeFromWorldPoint(startPos);
            //Debug.Log(startNode.isWalkable);
            //Debug.Log("시작 그리드 좌표 : (" + startNode.gridX + " , " + startNode.gridY + " , " + startNode.gridZ + ")");
            if (!startNode.isWalkable && Physics.CheckSphere(startPos, 0.5f, 9)) {
                startNode.isWalkable = true;
            }

            //Debug.Log("도착 노드 : ");
            //Debug.Log("타겟 월드좌표: " + targetPos);
            Node3D targetNode = grid.GetNodeFromWorldPoint(targetPos);
            //Debug.Log(targetNode.isWalkable);
            //Debug.Log("타겟 그리드 좌표 : (" + targetNode.gridX + ", " + targetNode.gridY + " , " + targetNode.gridZ + ")");
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
                            //Debug.Log("이웃 그리드 좌표 : " + neighbour.gridX + ", " + neighbour.gridY + ", " + neighbour.gridZ);
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
                //Debug.Log("경로 길이 : " + waypoints.Length);
            }
            //Debug.Log(waypoints);
            requestManager.FinishedProcessingPath(waypoints, pathSuccess);
        }

        // 탐색된 경로에 LadderNode 가 단 하나라도 포함되어 있을 경우 경로를 재구성함
        // 사다리를 이용하는 경우 사다리의 꼭대기 지점과 사다리의 바닥 지점을 반드시 통과하도록 해야함
        // 문제는 사다리의 꼭대기 지점과 바닥지점을 어떻게 경로에 추가할 것인가
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
                //Debug.Log("사다리에서 시작");
                ladderStart = true;
            }
            int index = 0;

            // 경로 중간에 사다리노드가 있는경우
            // 경로가 사다리에서 시작되는 경우
            // 경로가 사다리에서 끝나는 경우
            // 경로의 시작과 끝이 사다리 인경우

            // ladderNode 가 시작되는 지점과 끝나는지점 모두 찾아야함
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

            //기즈모 확인용
            grid.path = path;

            Vector3[] waypoints = SimplifyPath(path);
            Array.Reverse(waypoints);
            gizmoPoints = waypoints;
            return waypoints;
        }

        List<Node3D> AddLadderNode(Node3D currentLadderFirst, int indexCurrentLadderLast, int indexCurrentLadderFirst, Node3D currentLadderLast, bool ladderStart, List<Node3D> path) {
            if (currentLadderFirst.ladder == null) {
                Debug.LogError("사디리 노드에 사다리 정보가 없음");
                return path;
            } else {
                // 사다리의 진행 방향을 검사
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
                    //Debug.Log("꼭대기 시작");
                } else {
                    searchingDirection = 1;
                    //Debug.Log("바닥 시작");
                }


                while (true) {
                    currentPosition = new(currentPosition.x, currentPosition.y + searchingDirection, currentPosition.z);
                    Node3D currentNode = grid.GetNodeFromWorldPoint(currentPosition);
                    if (!currentNode.isLadder) {
                        Debug.LogError("탐색 방향 오류 : 사다리를 벗어남");
                        return path;
                    } else if (!currentNode.ladder.isTop == startingLadder.isTop) {
                        otherSideLadder = currentNode.ladder;
                        break;
                    }
                }
                List<Node3D> newPath = new();
                //Debug.Log("기존 Path 길이 : " + path.Count);
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

        // 경로를 간략화 함
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
                    // 이동해야 하는 방향이 일치하지않는다면 웨이포인트에 해당 좌표를 추가한다.
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

            if (markingLadder != null) {
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(markingLadder.worldPos, 0.5f);
            }
        }
    }
}
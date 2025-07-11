using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Node {
    static int nextId;
    public readonly int id;

    public float f, g, h;
    public Node from; // 어떤 노드로부터 이 노드에 도달했는지

    public List<Edge> edges = new();

    // 현재 노드와 대응되는 옥트리의 노드
    public OctreeNode octreeNode;

    public Node(OctreeNode octreeNode) {
        this.id = nextId++;
        this.octreeNode = octreeNode;
    }

    public override bool Equals(object obj) => obj is Node other && id == other.id;
    public override int GetHashCode() => id.GetHashCode();
}

public class Edge {
    public readonly Node a, b;

    public Edge(Node a, Node b) {
        this.a = a;
        this.b = b;
    }

    // 같은 간선인지 확인
    // a - b 와 b - a 는 같은 간선
    public override bool Equals(object obj) {
        return obj is Edge other && ((a == other.a && b == other.b) || (a == other.b && b == other.a));
    }

    // a - b 와 b - a 가 같은 간선이므로
    // 노드 a와 노드 b를 잇는 간선의 해시코드를 XOR 연산을 이용해 설정
    public override int GetHashCode() => a.GetHashCode() ^ b.GetHashCode();
}

public class Graph {
    public readonly Dictionary<OctreeNode, Node> nodes = new();
    public readonly HashSet<Edge> edges = new();

    List<Node> pathList = new();

    public int GetPathLength() => pathList.Count;

    public OctreeNode GetPathNode(int index) {
        if (pathList == null) return null;

        if (index < 0 || index >= pathList.Count) {
            //Debug.LogError($"인덱스가 범위를 벗어났습니다. 경로 길이: {pathList.Count}, 인덱스 : {index}");
            return null;
        }
        return pathList[index].octreeNode;
    }

    int maxIterations = 1000;
    public bool AStar(OctreeNode startNode, OctreeNode endNode) {
        pathList.Clear();
        Node start = FindNode(startNode);
        Node end = FindNode(endNode);

        if (start == null || end == null) {
            //Debug.LogError("시작 노드 혹은 끝 노드를 그래프 내에서 찾을 수 없습니다.");
            return false;
        }

        SortedSet<Node> openSet = new(new NodeComparer());
        HashSet<Node> closedSet = new();
        int iterationCount = 0;

        start.g = 0;
        start.h = Heuristic(start, end);
        start.f = start.g + start.h;
        start.from = null;
        openSet.Add(start);

        while (openSet.Count > 0) {
            if (++iterationCount > maxIterations) {
                //Debug.LogError("최고 반복 횟수를 초과했습니다.");
                return false;
            }

            Node current = openSet.First();
            openSet.Remove(current);

            if (current.Equals(end)) {
                ReconstructPath(current);
                return true;
            }

            closedSet.Add(current);

            foreach (Edge edge in current.edges) {
                Node neighbour = Equals(edge.a, current) ? edge.b : edge.a;
                if (closedSet.Contains(neighbour)) continue;

                float tentative_gScore = current.g + Heuristic(current, neighbour);

                if (tentative_gScore < neighbour.g || !openSet.Contains(neighbour)) {
                    neighbour.g = tentative_gScore;
                    neighbour.h = Heuristic(neighbour, end);
                    neighbour.f = neighbour.g + neighbour.h;
                    neighbour.from = current;
                    openSet.Add(neighbour);
                }
            }
        }

        //Debug.Log("탐색된 경로 없음.");
        return false;
    }

    void ReconstructPath(Node current) {
        while (current != null) {
            pathList.Add(current);
            current = current.from;
        }
        pathList.Reverse();
    }

    float Heuristic(Node a, Node b) => (a.octreeNode.bounds.center - b.octreeNode.bounds.center).sqrMagnitude;

    public class NodeComparer : IComparer<Node> {
        public int Compare(Node x, Node y) {
            if (x == null || y == null) return 0;

            int compare = x.f.CompareTo(y.f);
            if (compare == 0) {
                return x.id.CompareTo(y.id);
            }
            return compare;
        }
    }

    public void AddNode(OctreeNode octreeNode) {
        if (!nodes.ContainsKey(octreeNode)) {
            nodes.Add(octreeNode, new Node(octreeNode));
        }
    }

    public void AddEdge(OctreeNode a, OctreeNode b) {
        Node nodeA = FindNode(a);
        Node nodeB = FindNode(b);

        if (nodeA == null || nodeB == null) return;

        var edge = new Edge(nodeA, nodeB);
        if (edges.Add(edge)) {
            nodeA.edges.Add(edge);
            nodeB.edges.Add(edge);
        }
    }

    //public void DrawGraph() {
    //    Gizmos.color = Color.red;
    //    foreach (Edge edge in edges) {
    //        Gizmos.DrawLine(edge.a.octreeNode.bounds.center, edge.b.octreeNode.bounds.center);
    //    }
    //    foreach (var node in nodes.Values) {
    //        Gizmos.DrawWireSphere(node.octreeNode.bounds.center, 0.2f);
    //    }
    //}

    Node FindNode(OctreeNode octreeNode) {
        nodes.TryGetValue(octreeNode, out Node node);
        return node;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octree {
    public OctreeNode root;
    public Bounds bounds;
    public Graph graph;
    List<OctreeNode> emptyLeaves = new();

    // 트리에 포함될 모든 게임오브젝트와 트리 노드의 사이즈의 하한값을 인자로 받음
    public Octree(GameObject[] worldObjects, float minNodeSize, Graph graph) {
        this.graph = graph;

        CalculateBounds(worldObjects);
        CreateTree(worldObjects, minNodeSize);
        GetEmptyLeaves(root);
        GetEdges();
        Debug.Log(graph.edges.Count);
    }

    public OctreeNode FindClosestNode(Vector3 position) => FindClosestNode(root, position);

    // 옥트리의 노드와 좌표를 인자로 받음
    // 옥트리 노드의 자식 수만큼 순회하면서 자식노드 중 해당 좌표를 포함하는 경계를 가진 노드가 있는지 검사
    // 좌표를 포함하는 경계를 가진 자식노드가 잎노드라면 완료
    // 아니라면 해당 노드의 자식에 대해 이를 재귀 반복
    public OctreeNode FindClosestNode(OctreeNode node, Vector3 position) {
        OctreeNode found = null;
        for (int i = 0; i < node.children.Length; i++) {
            if (node.children[i].bounds.Contains(position)) {
                if (node.children[i].IsLeaf) {
                    found = node.children[i];
                    break;
                }
                found = FindClosestNode(node.children[i], position);
            }
        }
        return found;
    }

    void GetEdges() {
        foreach (OctreeNode leaf in emptyLeaves) {
            foreach (OctreeNode otherLeaf in emptyLeaves) {
                if (leaf.bounds.Intersects(otherLeaf.bounds)) {
                    graph.AddEdge(leaf, otherLeaf);
                }
            }
        }
    }

    // 비어있는, 갈수있는 노드를 구한다.
    void GetEmptyLeaves(OctreeNode node) {
        if (node.IsLeaf && node.objects.Count == 0) {
            emptyLeaves.Add(node);
            graph.AddNode(node);
            return;
        }

        if (node.children == null) return;

        foreach (OctreeNode child in node.children) {
            GetEmptyLeaves(child);
        }

        // 자식 노드들 사이의 모든 간선을 추가
        for (int i = 0; i < node.children.Length; i++) {
            for (int j = i + 1; j < node.children.Length; j++) {
                graph.AddEdge(node.children[i], node.children[j]);
            }
        }
    }

    void CreateTree(GameObject[] worldObjects, float minNodeSize) {
        root = new OctreeNode(bounds, minNodeSize);

        foreach (var obj in worldObjects) {
            root.Divide(obj);
        }
    }

    // 전체 경계를 구한다
    void CalculateBounds(GameObject[] worldObjects) {
        foreach (var obj in worldObjects) {
            bounds.Encapsulate(obj.GetComponent<Collider>().bounds);
        }

        // bound 의 모양을 정육면체로 만들기위해 사이즈를 구함
        Vector3 size = 0.6f * Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z) * Vector3.one;
        // 경계의 중심으로부터 좌측하단 뒤 까지의 거리와 중심으로부터 우측 상단 앞 까지의 거리를 설정
        bounds.SetMinMax(bounds.center - size, bounds.center + size);
    }
}

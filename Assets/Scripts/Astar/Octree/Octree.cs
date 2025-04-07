using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octree {
    public OctreeNode root;
    public Bounds bounds;
    public Graph graph;
    List<OctreeNode> emptyLeaves = new();

    // Ʈ���� ���Ե� ��� ���ӿ�����Ʈ�� Ʈ�� ����� �������� ���Ѱ��� ���ڷ� ����
    public Octree(GameObject[] worldObjects, float minNodeSize, Graph graph) {
        this.graph = graph;

        CalculateBounds(worldObjects);
        CreateTree(worldObjects, minNodeSize);
        GetEmptyLeaves(root);
        GetEdges();
        Debug.Log(graph.edges.Count);
    }

    public OctreeNode FindClosestNode(Vector3 position) => FindClosestNode(root, position);

    // ��Ʈ���� ���� ��ǥ�� ���ڷ� ����
    // ��Ʈ�� ����� �ڽ� ����ŭ ��ȸ�ϸ鼭 �ڽĳ�� �� �ش� ��ǥ�� �����ϴ� ��踦 ���� ��尡 �ִ��� �˻�
    // ��ǥ�� �����ϴ� ��踦 ���� �ڽĳ�尡 �ٳ���� �Ϸ�
    // �ƴ϶�� �ش� ����� �ڽĿ� ���� �̸� ��� �ݺ�
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

    // ����ִ�, �����ִ� ��带 ���Ѵ�.
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

        // �ڽ� ���� ������ ��� ������ �߰�
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

    // ��ü ��踦 ���Ѵ�
    void CalculateBounds(GameObject[] worldObjects) {
        foreach (var obj in worldObjects) {
            bounds.Encapsulate(obj.GetComponent<Collider>().bounds);
        }

        // bound �� ����� ������ü�� ��������� ����� ����
        Vector3 size = 0.6f * Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z) * Vector3.one;
        // ����� �߽����κ��� �����ϴ� �� ������ �Ÿ��� �߽����κ��� ���� ��� �� ������ �Ÿ��� ����
        bounds.SetMinMax(bounds.center - size, bounds.center + size);
    }
}

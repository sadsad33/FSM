using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 8���� �ڽĳ�带 ���� Ʈ��
// ��������� 3D ������ 8���� ���� �������� ����
public class OctreeGenerator : MonoBehaviour {
    public GameObject[] objects;
    public float minNodeSize = 1f;
    public Octree ot;

    public readonly Graph waypoints = new();

    void Awake() {
        ot = new(objects, minNodeSize, waypoints);
    }

    void OnDrawGizmos() {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(ot.bounds.center, ot.bounds.size);

        ot.root.DrawNode();
        //ot.graph.DrawGraph();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 8개의 자식노드를 갖는 트리
// 재귀적으로 3D 공간을 8개의 작은 영역으로 나눔
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

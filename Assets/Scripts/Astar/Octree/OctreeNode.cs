using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctreeNode {
    // 노드에 존재하는 오브젝트들의 리스트
    public List<OctreeObject> objects = new();

    static int nextId;
    public readonly int id;

    LayerMask unWalkableMask = 1 << 8;
    LayerMask walkableMask = 1 << 6 | 1 << 9 | 1 << 10 | 1 << 11;
    public bool isWalkable;
    public Bounds bounds;

    Bounds[] childBounds = new Bounds[8];
    public OctreeNode[] children;
    public bool IsLeaf => children == null;

    float minNodeSize;

    public OctreeNode(Bounds bounds, float minNodeSize) {
        id = nextId++;

        this.bounds = bounds;
        this.minNodeSize = minNodeSize;
        Vector3 newSize = bounds.size * 0.5f; // 자식의 사이즈는 부모사이즈의 절반
        Vector3 centerOffset = bounds.size * 0.25f; // 경계 중앙으로 부터의 오프셋
        Vector3 parentCenter = bounds.center;

        // 8개의 자식 노드들의 위치 설정
        for (int i = 0; i < 8; i++) {
            Vector3 childCenter = parentCenter;
            childCenter.x += centerOffset.x * ((i & 1) == 0 ? -1 : 1);
            childCenter.y += centerOffset.y * ((i & 2) == 0 ? -1 : 1);
            childCenter.z += centerOffset.z * ((i & 4) == 0 ? -1 : 1);
            childBounds[i] = new Bounds(childCenter, newSize);
        }
    }

    // 게임 오브젝트를 받아와 새로운 옥트리 오브젝트를 생성
    public void Divide(GameObject obj) => Divide(new OctreeObject(obj));

    void Divide(OctreeObject octreeObject) {
        // 경계의 사이즈가 최소 노드 사이즈 이하라면 더이상 나눌필요 없음
        // 현재 옥트리 노드의 오브젝트로 추가
        if (bounds.size.x <= minNodeSize) {
            if ((octreeObject.obj.layer & unWalkableMask) != 0) {
                isWalkable = false;
            }
            AddObject(octreeObject);
            return;
        }

        // 자식 노드 배열이 아직 초기화 되지 않았다면 초기화
        children ??= new OctreeNode[8];

        // 자식 노드의 경계와 오브젝트가 겹치는지 여부 판단
        bool intersectedChild = false;
        // 게임 오브젝트가 노드의 경계와 겹친다면(intersect) 계속해서 분할
        for (int i = 0; i < 8; i++) {
            children[i] ??= new OctreeNode(childBounds[i], minNodeSize);

            if (octreeObject.Intersects(childBounds[i])) {
                children[i].Divide(octreeObject);
                intersectedChild = true;
            }
        }

        // 자식노드들의 경계중 그 어떤 것과도 겹치지 않으면 이 노드의 오브젝트로 추가
        if (!intersectedChild) {
            AddObject(octreeObject);
        }
    }

    void AddObject(OctreeObject octreeObject) => objects.Add(octreeObject);

    public void MarkWalkableNode() {
        // 노드 사이즈가 최소 사이즈라면 해당 경계에 오브젝트가 존재한다는 뜻
        if (IsLeaf || bounds.size.x <= minNodeSize) {
            Ray downRay = new(bounds.center, Vector3.down);
            if (Physics.Raycast(downRay, bounds.size.y, walkableMask)) {
                isWalkable = true;
            }
            Ray upRay = new(bounds.center, Vector3.up);
            if (Physics.Raycast(upRay, 3f, unWalkableMask)){
                isWalkable = false;
            }
            return;
        }

        for (int i = 0; i < 8; i++) {
            children[i] ??= new OctreeNode(childBounds[i], minNodeSize);
            children[i].MarkWalkableNode();
        }
    }

    public void DrawNode() {
        //Gizmos.color = Color.green;
        //Gizmos.DrawWireCube(bounds.center, bounds.size);
        //foreach (OctreeObject obj in objects) {
        //    if (obj.Intersects(bounds)) {
        //        Gizmos.color = Color.red;
        //        Gizmos.DrawCube(bounds.center, bounds.size);
        //    }
        //}

        //if (!isWalkable) {
        //    Gizmos.color = Color.cyan;
        //    Gizmos.DrawWireCube(bounds.center, bounds.size);
        //}
        if (isWalkable) {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }

        if (children != null) {
            foreach (OctreeNode child in children) {
                if (child != null) child.DrawNode();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctreeNode {
    // ��忡 �����ϴ� ������Ʈ���� ����Ʈ
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
        Vector3 newSize = bounds.size * 0.5f; // �ڽ��� ������� �θ�������� ����
        Vector3 centerOffset = bounds.size * 0.25f; // ��� �߾����� ������ ������
        Vector3 parentCenter = bounds.center;

        // 8���� �ڽ� ������ ��ġ ����
        for (int i = 0; i < 8; i++) {
            Vector3 childCenter = parentCenter;
            childCenter.x += centerOffset.x * ((i & 1) == 0 ? -1 : 1);
            childCenter.y += centerOffset.y * ((i & 2) == 0 ? -1 : 1);
            childCenter.z += centerOffset.z * ((i & 4) == 0 ? -1 : 1);
            childBounds[i] = new Bounds(childCenter, newSize);
        }
    }

    // ���� ������Ʈ�� �޾ƿ� ���ο� ��Ʈ�� ������Ʈ�� ����
    public void Divide(GameObject obj) => Divide(new OctreeObject(obj));

    void Divide(OctreeObject octreeObject) {
        // ����� ����� �ּ� ��� ������ ���϶�� ���̻� �����ʿ� ����
        // ���� ��Ʈ�� ����� ������Ʈ�� �߰�
        if (bounds.size.x <= minNodeSize) {
            if ((octreeObject.obj.layer & unWalkableMask) != 0) {
                isWalkable = false;
            }
            AddObject(octreeObject);
            return;
        }

        // �ڽ� ��� �迭�� ���� �ʱ�ȭ ���� �ʾҴٸ� �ʱ�ȭ
        children ??= new OctreeNode[8];

        // �ڽ� ����� ���� ������Ʈ�� ��ġ���� ���� �Ǵ�
        bool intersectedChild = false;
        // ���� ������Ʈ�� ����� ���� ��ģ�ٸ�(intersect) ����ؼ� ����
        for (int i = 0; i < 8; i++) {
            children[i] ??= new OctreeNode(childBounds[i], minNodeSize);

            if (octreeObject.Intersects(childBounds[i])) {
                children[i].Divide(octreeObject);
                intersectedChild = true;
            }
        }

        // �ڽĳ����� ����� �� � �Ͱ��� ��ġ�� ������ �� ����� ������Ʈ�� �߰�
        if (!intersectedChild) {
            AddObject(octreeObject);
        }
    }

    void AddObject(OctreeObject octreeObject) => objects.Add(octreeObject);

    public void MarkWalkableNode() {
        // ��� ����� �ּ� �������� �ش� ��迡 ������Ʈ�� �����Ѵٴ� ��
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

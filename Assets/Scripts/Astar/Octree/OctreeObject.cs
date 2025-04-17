using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��Ʈ���� ���ԵǴ� ������Ʈ��
public class OctreeObject{
    // Ʈ���� ������Ʈ�� ���� ���� ��踦 ���´�
    Bounds bounds;

    public GameObject obj;
    public OctreeObject(GameObject obj) {
        this.obj = obj;
        bounds = obj.GetComponent<Collider>().bounds;
    }

    // �࿡ ���ĵ� �ڽ��� ��谣�� ��ġ���� Ȯ��
    // �޼����� ���ڷ� ���޵� ���� �� ������Ʈ�� ��迡 ���� üũ
    public bool Intersects(Bounds boundsToCheek) => bounds.Intersects(boundsToCheek);
}

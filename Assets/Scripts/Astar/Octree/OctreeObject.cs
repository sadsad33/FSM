using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 옥트리에 포함되는 오브젝트들
public class OctreeObject{
    // 트리의 오브젝트들 또한 고유 경계를 갖는다
    Bounds bounds;

    public GameObject obj;
    public OctreeObject(GameObject obj) {
        this.obj = obj;
        bounds = obj.GetComponent<Collider>().bounds;
    }

    // 축에 정렬된 박스의 경계간에 겹치는지 확인
    // 메서드의 인자로 전달된 경계와 이 오브젝트의 경계에 대해 체크
    public bool Intersects(Bounds boundsToCheek) => bounds.Intersects(boundsToCheek);
}

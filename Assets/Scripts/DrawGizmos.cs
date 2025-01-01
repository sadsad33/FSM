using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmos : MonoBehaviour
{
    public float radius;
    public Vector3 size;
    public Color color;
    [Header("0 : Shpere , 1 : WireSphere, 2 : Cube, 3 : WireCube")]
    public int shape;

    private void OnDrawGizmos() {
        switch (shape) {
            case 0:
                Gizmos.color = color;
                Gizmos.DrawSphere(transform.position, radius);
                break;
            case 1:
                Gizmos.color = color;
                Gizmos.DrawWireSphere(transform.position, radius);
                break;
            case 2:
                Gizmos.color = color;
                Gizmos.DrawCube(transform.position,size);
                break;
            case 3:
                Gizmos.color = color;
                Gizmos.DrawWireCube(transform.position, size);
                break;
        }
    }
}

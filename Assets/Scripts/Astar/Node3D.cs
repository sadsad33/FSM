using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node3D : IHeapItem<Node3D>
{
    public bool isWalkable;
    public bool isAscendable;
    public Vector3 worldPos;
    public int gridX, gridY, gridZ;
    public int movementPenalty;
    public int gCost, hCost;
    public Node3D parentNode;
    int heapIndex;

    public Node3D(bool _isAscendable, bool _isWalkable, Vector3 _worldPos, int _gridX, int _gridY, int _gridZ, int _penalty) {
        isAscendable = _isAscendable;
        isWalkable = _isWalkable;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
        gridZ = _gridZ;
        movementPenalty = _penalty;
    }
   
    public int fCost {
        get { return gCost + hCost; }
    }

    public int HeapIndex {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    public int CompareTo(Node3D nodeToCompare) {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
            compare = hCost.CompareTo(nodeToCompare.hCost);
        return -compare;
    }
}

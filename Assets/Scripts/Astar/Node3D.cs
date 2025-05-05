using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class Node3D : IHeapItem<Node3D> {
        public bool isWalkable;
        public bool isInclined;
        public bool isLadder;
        public Vector3 worldPos;
        public int gridX, gridY, gridZ;
        public int movementPenalty;
        public int gCost, hCost;
        public Node3D parentNode;
        protected int heapIndex;

        public Ladder ladder;

        public Node3D(bool _isInclined, bool _isWalkable, Vector3 _worldPos, int _gridX, int _gridY, int _gridZ, int _penalty) {
            isInclined = _isInclined;
            isWalkable = _isWalkable;
            worldPos = _worldPos;
            gridX = _gridX;
            gridY = _gridY;
            gridZ = _gridZ;
            movementPenalty = _penalty;
        }

        public Node3D(bool _isWalkable, bool _isLadder, Vector3 _worldPos) {
            worldPos = _worldPos;
            isLadder = _isLadder;
            isWalkable = _isWalkable;
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
}
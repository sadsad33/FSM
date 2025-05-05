using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class LadderNode : Node3D {

        Ladder ladder;
        public LadderNode(bool _isInclined, bool _isWalkable, Vector3 _worldPos, int _gridX, int _gridY, int _gridZ, int _penalty)
            : base(_isInclined, _isWalkable, _worldPos, _gridX, _gridY, _gridZ, _penalty) {
        }

        public void SetLadder(Ladder _ladder) {
            ladder = _ladder;
        }

        public Ladder GetLadder() {
            return ladder;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkNode : Node3D {
    public LinkNode(bool _isInclined, bool _isWalkable, Vector3 _worldPos, int _gridX, int _gridY, int _gridZ, int _penalty) : base(_isInclined, _isWalkable, _worldPos, _gridX, _gridY, _gridZ, _penalty) {
        
    }
}
    

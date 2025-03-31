using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterStatsManager : CharacterStatsManager {
        public float detectionRadius;
        public float attackDistance;
        public LayerMask detectionLayer;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterStatsManager : CharacterStatsManager {
        public float DetectionRadius {
            get; set;
        }
        
        public float AttackDistance {
            get; set;
        }

        public float CombatStanceDistance {
            get; set;
        }

        public LayerMask detectionLayer;
        public bool hasDrawnWeapon = false;
    }
}
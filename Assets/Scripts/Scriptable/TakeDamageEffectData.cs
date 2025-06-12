using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    [CreateAssetMenu(menuName = "Character Effects/Take Damage")]
    public class TakeDamageEffectData : CharacterEffectData {

        public float physicalDamage = 0;

        public float poiseDamage = 0;

        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        
    }
}

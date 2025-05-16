using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    [CreateAssetMenu(menuName = "Character Effects/Take Damage Effect")]
    public class TakeDamageEffect : CharacterEffect {

        // TODO
        // Sound Effects
        // Elemental Damage
        public CharacterManager characterDamageFrom;

        public float physicalDamage = 0;

        public float poiseDamage = 0;
        public bool poiseIsBroken = false;

        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        public float angleHitFrom;
        public Vector3 contactPoint;

        public override void ProcessEffect(CharacterManager character) {
            if (character.characterStatsManager.isDead) return;
            if (character.isInvulnerable) return;

            //CalculateDamage(character);
        }
    }
}
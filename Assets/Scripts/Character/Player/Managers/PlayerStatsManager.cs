using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerStatsManager : CharacterStatsManager {
        protected override void Awake() {
            base.Awake();
        }

        public void DeductStamina(float deduct) {
            currentStamina -= deduct;
            if (currentStamina <= 0f) currentStamina = 0f;
        }

        public void RegenerateStamina(float regenerate) {
            if (currentStamina >= maxStamina) return;
            currentStamina += regenerate;
        }
    }
}

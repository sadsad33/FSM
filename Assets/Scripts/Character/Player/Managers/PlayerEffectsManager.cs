using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerEffectsManager : CharacterEffectsManager {

        protected override void Awake() {
            base.Awake();
        }

        public override void ProcessEffectInstantly(CharacterEffect effect) {
            base.ProcessEffectInstantly(effect);
        }
    }
}

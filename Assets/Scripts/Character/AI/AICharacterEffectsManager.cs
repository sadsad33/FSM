using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterEffectsManager : CharacterEffectsManager {

        AICharacterManager aiCharacter;
        protected override void Awake() {
            base.Awake();
            aiCharacter = GetComponent<AICharacterManager>();
        }

        public override void ProcessEffectInstantly(CharacterEffect effect) {
            base.ProcessEffectInstantly(effect);
            effect.ProcessEffect(character);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class CharacterEffectsManager : MonoBehaviour {
        public CharacterManager character;

        protected virtual void Awake() {
            character = GetComponent<CharacterManager>();
        }

        public virtual void ProcessEffectInstantly(CharacterEffect effect) {
            effect.ProcessEffect(character);
        }
    }
}

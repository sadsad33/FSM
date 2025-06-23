using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class CharacterEffect {
        readonly CharacterEffectData data;
        public CharacterEffect(CharacterEffectData data) {
            this.data = data;
        }

        public virtual void ProcessEffect(CharacterManager character) {
            
        }

        public CharacterEffectData GetEffectData() => data;
    }
}

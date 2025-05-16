using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class CharacterEffectsManager : MonoBehaviour {
        CharacterManager character;

        public virtual void ProcessEffectInstantly(CharacterEffect effect) {
            effect.ProcessEffect(character);
        }
    }
}

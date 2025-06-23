using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class CharacterEffectsManager : MonoBehaviour {
        public CharacterManager character;
        [SerializeField] protected CharacterEffect currentEffect;
        
        protected virtual void Awake() {
            character = GetComponent<CharacterManager>();
        }

        public virtual void ProcessEffectInstantly(CharacterEffect effect) {
            currentEffect = effect;
                 
            //effect.ProcessEffect(character);
        }

        public CharacterEffect GetCurrentEffect() => currentEffect;
    }
}

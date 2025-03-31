using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterMovementState : CharacterMovementState {
        protected AICharacterManager aiCharacter;
        protected AICharacterEyesManager aiCharacterEyes;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            aiCharacter = character as AICharacterManager;
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            Thinking();
        }

        public override void Exit(CharacterManager character) {
            
        }

        public virtual void Thinking() {
            
        }
    }
}

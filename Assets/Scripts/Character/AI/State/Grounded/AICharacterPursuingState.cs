using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterPursuingState : AICharacterGroundedState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            aiCharacterEyes = aiCharacter.aiEyesManager;
            moveSpeedModifier = 0.9f;
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            aiCharacter.characterAnimatorManager.animator.SetFloat("Vertical", 1f, 0.1f, Time.deltaTime);
            HandleMovement();
        }

        public override void Exit(CharacterManager character) {
        
        }

        public override void Thinking() {
            
        }

        private void HandleMovement() {
            aiCharacter.StartMoving();
        }
    }
}

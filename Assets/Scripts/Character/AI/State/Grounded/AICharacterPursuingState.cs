using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterPursuingState : AICharacterGroundedState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            aiCharacterEyes = aiCharacter.aiEyesManager;
            moveSpeedModifier = 5f;
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
            moveDirection = aiCharacter.currentWaypoint - aiCharacter.transform.position;
            moveDirection.y = 0;
            moveDirection.Normalize();
            Vector3 temp = moveDirection;
            temp.y = 0;
            if (temp != Vector3.zero) {
                aiCharacter.transform.forward = temp;
            }
            aiCharacter.cc.Move(moveSpeedModifier * Time.deltaTime * moveDirection);

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterIdlingState : AICharacterGroundedState {
        
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            aiCharacterEyes = aiCharacter.aiEyesManager;
            moveSpeedModifier = 0f;
        }
        public override void Stay(CharacterManager character) {
            base.Stay(character);
            character.cc.Move(Vector3.zero);
            character.characterAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
        }

        public override void Exit(CharacterManager character) {
            
        }

        public override void Thinking() {
            SelectTarget();
        }

        private void SelectTarget() {
            //if (aiCharacterEyes.targetAround == null && aiCharacterEyes.targetPossible == null) return;
            //else if (aiCharacterEyes.targetAround != null) {
            //    aiCharacterEyes.currentTarget = aiCharacterEyes.targetAround;
            //    aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiPursuingState);
            //} else {
            //    aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiBewaringState);
            //}
            if (aiCharacterEyes.currentTarget != null) {
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiPursuingState);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterClimbingUpState : AICharacterGroundedState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            aiCharacter.isPerformingAction = true;
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            aiCharacter.aiAnimatorManager.animator.SetFloat("Vertical", 1f, 0.1f, Time.deltaTime);
        }

        public override void Exit(CharacterManager character) {
        }

        public override void Thinking() {
            base.Thinking();
            if (!aiCharacter.isPerformingAction) {
                if (aiCharacter.rightFootUp) {
                    aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiRightFootUpIdlingState);
                } else {
                    aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiLeftFootUpIdlingState);
                }
            }
        }
    }
}
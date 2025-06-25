using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterHasBeenParriedState : AICharacterGroundedState {

        public override void Enter(CharacterManager character) {
            base.Enter(character);
            aiCharacter.isPerformingAction = true;
            aiCharacter.aiAnimatorManager.PlayAnimation("Parry_Parried", aiCharacter.isPerformingAction);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
        }

        public override void Thinking() {
            base.Thinking();
            if (!aiCharacter.isPerformingAction) {
                aiCharacter.hasBeenParried = false;
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiIdlingState);
            }
        }
    }
}

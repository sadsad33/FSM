using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterHasBeenParriedState : AICharacterGroundedState {

        public override void Enter(CharacterManager character) {
            base.Enter(character);
            aiCharacter.isPerformingAction = true;
            aiCharacter.aiInteractionManager.riposteCollider.SetActive(true);
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
            if (aiCharacter.beingRiposted) {
                aiCharacter.hasBeenParried = false;
                aiCharacter.aiInteractionManager.riposteCollider.SetActive(false);
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiBeingRipostedState);
            } else if (!aiCharacter.isPerformingAction) {
                aiCharacter.hasBeenParried = false;
                aiCharacter.aiInteractionManager.riposteCollider.SetActive(false);
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiIdlingState);
            }
        }
    }
}

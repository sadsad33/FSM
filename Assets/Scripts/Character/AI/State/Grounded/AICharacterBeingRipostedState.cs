using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterBeingRipostedState : AICharacterGroundedState {

        public override void Enter(CharacterManager character) {
            base.Enter(character);
            aiCharacter.isPerformingAction = true;
            aiCharacter.beingRiposted = false;
            aiCharacter.aiAnimatorManager.PlayAnimation("Parry_Stabbed", aiCharacter.isPerformingAction);
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
                if (aiCharacter.aiStatsManager.isDead) {
                    
                } else
                    aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiRipostedGetUpState);
            }
        }
    }
}

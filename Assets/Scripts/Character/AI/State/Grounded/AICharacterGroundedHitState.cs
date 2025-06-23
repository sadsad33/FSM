using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterGroundedHitState : AICharacterGroundedState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
        }

        public override void Thinking() {
            base.Thinking();
            if (!aiCharacter.isPerformingAction)
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiIdlingState);
        }
    }
}

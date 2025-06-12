using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterGroundedState : AICharacterMovementState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
        }

        public override void Thinking() {
            base.Thinking();
            if (!aiCharacter.isGrounded) {
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiFallingState);
            }
        }

    }
}

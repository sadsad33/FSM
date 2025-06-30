using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterBackstabbedGetUpState : AICharacterGroundedState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            aiCharacter.isPerformingAction = true;
            aiCharacter.aiAnimatorManager.PlayAnimation("Backstab_Stabbed_GetUp", aiCharacter.isPerformingAction);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            aiCharacter.transform.rotation *= aiCharacter.aiAnimatorManager.animator.deltaRotation;
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

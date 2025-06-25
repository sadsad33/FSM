using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterAttackState : AICharacterGroundedState {
        protected BTNode.Status result;
        public bool IsDoingComboAttack { get; set; }
        public Enums.CharacterBehaviourCode AIBehaviourStatus { get; set; }

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
            if (aiCharacter.hasBeenParried) {
                aiCharacter.acsm.ChangeState(aiCharacter.acsm.aiHasBeenParriedState);
            }
        }
    }
}
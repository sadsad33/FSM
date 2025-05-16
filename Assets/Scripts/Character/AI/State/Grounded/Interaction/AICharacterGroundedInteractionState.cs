using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterGroundedInteractionState : AICharacterGroundedState {
        protected Interactable curInteractable;
        protected AICharacterInteractionManager aiInteraction;
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            aiInteraction = aiCharacter.aiInteractionManager;
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
        }

        public override void Thinking() {
            base.Thinking();
        }
    }
}
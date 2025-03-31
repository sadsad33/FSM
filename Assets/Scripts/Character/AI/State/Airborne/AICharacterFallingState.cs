using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterFallingState : AICharacterAirborneState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            character.characterAnimatorManager.PlayAnimation("Falling", false);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            if (character.InAirTimer != 0) {
                
            }
        }
    }
}
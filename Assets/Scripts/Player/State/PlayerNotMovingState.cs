using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerNotMovingState : PlayerMovementState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
            
        }
    }
}

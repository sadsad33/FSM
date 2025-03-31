using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class AICharacterAirborneState : AICharacterMovementState {
        protected float inAirTimer;
        public Vector3 MovingVelocityInAir { get; set; }
        public override void Enter(CharacterManager character) {
            base.Enter(character);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {

        }

        public override void Thinking() {

        }
    }
}

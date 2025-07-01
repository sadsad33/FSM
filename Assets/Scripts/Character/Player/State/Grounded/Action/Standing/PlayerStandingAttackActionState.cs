using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerStandingAttackActionState : PlayerGroundedActionState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
        }

        public override void HandleInput() {
            base.HandleInput();
            if (player.hasBeenParried)
                player.psm.ChangeState(player.psm.hasBeenParriedState);
        }
    }
}
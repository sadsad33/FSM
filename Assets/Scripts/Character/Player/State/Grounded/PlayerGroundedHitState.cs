using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerGroundedHitState : PlayerGroundedState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            if (!player.isPerformingAction) player.isPerformingAction = true;
            if (player.isJumping) player.isJumping = false;
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
        }

        public override void Exit(CharacterManager character) {
            base.Exit(character);
        }

        public override void HandleInput() {
            base.HandleInput();
            if (!player.isPerformingAction) {
                player.pmsm.ChangeState(player.pmsm.idlingState);
            }
        }
    }
}
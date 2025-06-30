using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerStoppingState : PlayerGroundedState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.RunningStateTimer = 0f;
            if (!player.isAttacking && !player.isPerformingAction)
                player.isMoving = true;
            //player.playerInputManager.SprintInputTimer = 0f;
            //player.pasm.ChangeState(player.pasm.standingActionIdlingState);
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            player.canSliding = false;
        }

        public override void Exit(CharacterManager character) {
        }

        public override void HandleInput() {
            base.HandleInput();
        }
    }
}
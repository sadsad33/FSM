using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBH {
    public class PlayerGroundedState : PlayerMovementState {
        public override void Enter(CharacterManager character) {
            base.Enter(character);
            player.InAirTimer = 0f;
        }

        public override void Stay(CharacterManager character) {
            base.Stay(character);
            //Debug.Log("Grounded State Stay ÀÇ MoveDirection : " + moveDirection);
        }

        public override void Exit(CharacterManager character) {

        }

        public override void HandleInput() {
            base.HandleInput();
            //Debug.Log("Grounded State HandleInput ÀÇ MoveDirection : " + moveDirection);
            if (!player.isGrounded) {
                //if (player.InAirTimer >= 0.2f) {
                player.pmsm.ChangeState(player.pmsm.fallingState);
                player.pasm.ChangeState(player.pasm.airborneActionIdlingState);
                //}
            }
            else {
                if (player.playerInputManager.RollFlag) {
                    if (player.playerInputManager.SprintInputTimer > 0f && player.playerInputManager.SprintInputTimer < 0.3f) {
                        if (player.playerStatsManager.currentStamina > 0f)
                            player.pmsm.ChangeState(player.pmsm.rollingState);
                    }
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerMovementState {
    public override void Enter(CharacterManager character) {
        base.Enter(character);
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
        //Debug.Log("Grounded State Stay 의 MoveDirection : " + moveDirection);
    }

    public override void Exit(CharacterManager character) {

    }

    public override void HandleInput() {
        base.HandleInput();
        //Debug.Log("Grounded State HandleInput 의 MoveDirection : " + moveDirection);
        if (!player.isGrounded) {
            if (player.InAirTimer <= 0.3f) player.pmsm.ChangeState(player.pmsm.fallingState);
        } else if (!player.isPerformingAction) {
            if (player.playerInputManager.RollFlag) {
                if (player.playerInputManager.SprintInputTimer > 0f && player.playerInputManager.SprintInputTimer < 0.3f) {
                    player.pmsm.ChangeState(player.pmsm.rollingState);
                }
            } else if (player.playerInputManager.JumpInput) {
                if (moveAmount > 0f) {
                    player.pmsm.runningJumpState.MovingVelocityInAir = moveDirection;
                    //Debug.Log("달리기 점프 속도 : " + player.pmsm.runningJumpState.MovingVelocityInAir);
                    player.pmsm.ChangeState(player.pmsm.runningJumpState);
                } else {
                    player.pmsm.ChangeState(player.pmsm.standingJumpState);
                }
            } else if (player.playerInputManager.CrouchInput) {
                if (player.canSliding) player.pmsm.ChangeState(player.pmsm.slidingState);
                else {
                    if (!player.isCrouched) {
                        player.pmsm.ChangeState(player.pmsm.standToCrouchState);
                    }
                }
            }
        }
    }
}

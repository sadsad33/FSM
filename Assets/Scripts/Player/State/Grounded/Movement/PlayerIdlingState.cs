using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdlingState : PlayerGroundedState {

    public override void Enter(CharacterManager character) {
        base.Enter(character);
        player.pasm.ChangeState(player.pasm.standingActionIdlingState);
        player.RunningStateTimer = 0f;
        moveSpeedModifier = 0f;
        player.playerInputManager.SprintInputTimer = 0f;
        if (!sprintInputDelaySet)
            sprintInputDelaySet = true;
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
        player.cc.Move(Vector3.zero);
        player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
        //HandleMovement();
    }

    public override void Exit(CharacterManager character) {
    }

    public override void HandleInput() {
        base.HandleInput();
        if (player.playerInputManager.CrouchInput) {
            player.pmsm.ChangeState(player.pmsm.standToCrouchState);
        } else if (player.playerInputManager.JumpInput) {
            player.pmsm.ChangeState(player.pmsm.standingJumpState);
        } else if (moveAmount > 0f) {
            if (player.playerInputManager.WalkInput) {
                player.pmsm.ChangeState(player.pmsm.walkingState);
            } else {
                player.pmsm.ChangeState(player.pmsm.runningState);
            }
        }
    }

    protected override void HandleMovement() {
        base.HandleMovement();
        float speed = player.moveSpeed * moveSpeedModifier;
        //currentMovingSpeed = speed;
        moveDirection *= speed;
        if (moveDirection.magnitude > PlayerMaximumVelocity.magnitude)
            PlayerMaximumVelocity = moveDirection;
        player.cc.Move(moveDirection * speed);
    }
}

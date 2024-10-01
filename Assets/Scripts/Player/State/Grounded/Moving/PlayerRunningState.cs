using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunningState : PlayerMovingState {
   
    public override void Enter(CharacterManager character) {
        base.Enter(character);
        player.playerInputManager.SprintInputTimer = 0f;
        moveSpeedModifier = 1f;
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
        player.RunningStateTimer += Time.deltaTime;
        HandleGroundedMovement();
        player.playerAnimatorManager.animator.SetFloat("Vertical", 1f, 0.1f, Time.deltaTime);
    }

    public override void Exit(CharacterManager character) {
    }

    public override void HandleInput() {
        base.HandleInput();
        if (moveAmount <= 0f) {
            if (player.RunningStateTimer >= 0.5f)
                player.pmsm.ChangeState(player.pmsm.lightStoppingState);
            else
                player.pmsm.ChangeState(player.pmsm.idlingState);
        }
        else {
            if (player.playerInputManager.WalkInput) player.pmsm.ChangeState(player.pmsm.walkingState);
            if (player.playerInputManager.SprintInput && player.playerInputManager.SprintInputTimer >= 0.3f) player.pmsm.ChangeState(player.pmsm.sprintingState);
        }
    }

    protected override void HandleGroundedMovement() {
        base.HandleGroundedMovement();
        float speed = player.moveSpeed * moveSpeedModifier;
        moveDirection *= speed;
        moveDirection /= 200f;
        player.cc.Move(moveDirection);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdlingState : PlayerGroundedState {

    public override void Enter(CharacterManager character) {
        base.Enter(character);
        player.RunningStateTimer = 0f;
        moveSpeedModifier = 0f; 
        player.playerInputManager.SprintInputTimer = 0f;
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
        player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
        //HandleMovement();
    }

    public override void Exit(CharacterManager character) {
    }

    public override void HandleInput() {
        base.HandleInput();
        if (moveAmount > 0f) {
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
        currentMovingSpeed = speed;
        player.cc.Move(moveDirection * speed);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkingState : PlayerMovingState {
    
    public override void Enter(CharacterManager character) {
        base.Enter(character);
        moveSpeedModifier = 0.6f;
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
        player.playerAnimatorManager.animator.SetFloat("Vertical", 0.5f, 0.1f, Time.deltaTime);
        HandleGroundedMovement();
    }

    public override void Exit(CharacterManager characer) {
    }

    public override void HandleInput() {
        base.HandleInput();
        if (!player.playerInputManager.WalkInput) {
            player.pmsm.ChangeState(player.pmsm.idlingState);
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

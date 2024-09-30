using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdlingState : PlayerGroundedState {

    public override void Enter(CharacterManager character) {
        base.Enter(character);
        moveSpeedModifier = 0f;

        //TODO
        //Idle 상태의 애니메이션 재생
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
        player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
        HandleGroundedMovement();
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

    protected override void HandleGroundedMovement() {
        base.HandleGroundedMovement();
        float speed = player.moveSpeed * moveSpeedModifier;
        player.cc.Move(moveDirection * speed);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollingState : PlayerMovingState {

    public override void Enter(CharacterManager character) {
        base.Enter(character);
        player.isPerformingAction = true;
        player.playerAnimatorManager.PlayAnimation("Rolling", player.isPerformingAction);
        player.isInvulnerable = true;
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
        HandleGroundedMovement();
    }

    public override void Exit(CharacterManager character) {
        player.isInvulnerable = false;
        player.playerInputManager.SprintInputTimer = 0f;
        player.playerInputManager.RollFlag = false;
    }

    public override void HandleInput() {
        base.HandleInput();
        if (!player.isPerformingAction)
            player.pmsm.ChangeState(player.pmsm.idlingState);
    }

    protected override void HandleGroundedMovement() {
        base.HandleGroundedMovement();
        if (moveDirection == Vector3.zero) moveDirection = player.transform.forward;
        Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
        player.transform.rotation = rollRotation;
    }
}
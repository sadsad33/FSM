using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchedIdlingState : PlayerGroundedState
{
    public override void Enter(CharacterManager character) {
        base.Enter(character);
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
        player.cc.Move(Vector3.zero);
        player.playerAnimatorManager.animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
    }

    public override void Exit(CharacterManager character) {
    }

    public override void HandleInput() {
        base.HandleInput();
        if (!player.playerInputManager.CrouchInput) player.pmsm.ChangeState(player.pmsm.crouchToStandState);
        else if (moveAmount > 0f)
            player.pmsm.ChangeState(player.pmsm.crouchedWalkingState);
    }
}

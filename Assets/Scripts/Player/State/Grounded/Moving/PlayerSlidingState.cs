using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlidingState : PlayerMovingState {
    public override void Enter(CharacterManager character) {
        base.Enter(character);
        moveSpeedModifier = 2f;
        player.isPerformingAction = true;
        player.playerAnimatorManager.PlayAnimation("Sliding", player.isPerformingAction);
        //Debug.Log(player.canSliding);
    }

    public override void Stay(CharacterManager character) {
        base.Stay(character);
    }

    public override void Exit(CharacterManager character) {
        player.canSliding = false;
    }

    public override void HandleInput() {
        if (player.isPerformingAction) return;
        base.HandleInput();
        player.pmsm.ChangeState(player.pmsm.idlingState);
    }
}
